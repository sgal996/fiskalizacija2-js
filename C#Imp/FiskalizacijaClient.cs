using System;
using System.Threading.Tasks;

namespace Fiskalizacija2 {
    public class FiskalizacijaClient {
        private readonly XmlSigner _signer;
        private readonly FiskalizacijaOptions _options;

        public FiskalizacijaClient(FiskalizacijaOptions options) {
            _options = options ?? new FiskalizacijaOptions();
            _signer = new XmlSigner(options);
        }

        private async Task<FiskalizacijaResult<TReq, TRes>> Execute<TReqData, TReq, TRes>(
            TReqData zahtjev,
            RequestConfig<TReqData, TReq, TRes> config
        ) where TReq : SerializableRequest where TRes : ParsedResponse {
            var result = new FiskalizacijaResult<TReq, TRes> { Success = false };
            try {
                var requestInstance = zahtjev is TReq existing ? existing : config.RequestFactory!(zahtjev);
                result.ReqObject = requestInstance;
                var signedXml = _signer.SignFiscalizationRequest(requestInstance.ToXmlString(), requestInstance.Id);
                var soap = GenerateSoapEnvelope(signedXml);
                result.SoapReqRaw = soap;
                var (statusCode, data) = await HttpHelper.PostRequestAsync(soap, _options);
                result.HttpStatusCode = statusCode;
                result.SoapResRaw = data;
                try {
                    result.SoapResSignatureValid = XmlSigner.IsValidSignature(data);
                } catch (Exception e) {
                    result.SoapResSignatureValid = false;
                    result.Error = ErrorUtils.ParseError(e);
                }
                if (config.ResponseFactory != null) {
                    result.ResObject = config.ResponseFactory(data);
                }
                result.Success = result.Error == null;
            }
            catch (Exception ex) {
                result.Error = ErrorUtils.ParseError(ex);
            }
            return result;
        }

        private string GenerateSoapEnvelope(string body, bool withXmlDec = true) {
            var res = string.Empty;
            if (withXmlDec) {
                res += "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            }
            res += "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
            res += "<soap:Body>";
            res += body;
            res += "</soap:Body>";
            res += "</soap:Envelope>";
            return res;
        }
    }
}
