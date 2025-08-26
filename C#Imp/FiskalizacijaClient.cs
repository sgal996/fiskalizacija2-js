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
            TReq zahtjev,
            RequestConfig<TReqData, TReq, TRes> config
        ) where TReq : SerializableRequest where TRes : ParsedResponse {
            var result = new FiskalizacijaResult<TReq, TRes> { Success = false };
            try {
                result.ReqObject = zahtjev;
                var signedXml = _signer.SignFiscalizationRequest(zahtjev.ToXmlString(), zahtjev.Id);
                var soap = GenerateSoapEnvelope(signedXml);
                result.SoapReqRaw = soap;
                // TODO: send HTTP request and parse response
            }
            catch (Exception ex) {
                result.Error = ex;
            }
            return await Task.FromResult(result);
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
