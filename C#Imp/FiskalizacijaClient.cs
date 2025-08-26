using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Fiskalizacija2.Utils;

namespace Fiskalizacija2 {
    public class FiskalizacijaClient {
        private readonly XmlSigner _signer;
        private readonly FiskalizacijaOptions _options;

        public FiskalizacijaClient(FiskalizacijaOptions options) {
            _options = options ?? new FiskalizacijaOptions();
            _signer = new XmlSigner(options);
        }

        private async Task<FiskalizacijaResult<TReq, TRes>> Execute<TReqData, TReq, TRes>(
            object zahtjev,
            RequestConfig<TReqData, TReq, TRes> config
        ) where TReq : SerializableRequest where TRes : ParsedResponse {
            var result = new FiskalizacijaResult<TReq, TRes> { Success = false };
            try {
                var requestInstance = zahtjev is TReq existing
                    ? existing
                    : config.RequestFactory!((TReqData)zahtjev);
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
                    result.ResObject = XmlUtils.UsingXmlDocument(data, doc => {
                        var ns = new XmlNamespaceManager(new NameTable());
                        ns.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
                        ns.AddNamespace("efis", "http://www.porezna-uprava.gov.hr/fin/2024/types/eFiskalizacija");
                        ns.AddNamespace("eizv", "http://www.porezna-uprava.gov.hr/fin/2024/types/eIzvjestavanje");
                        var el = doc.XPathSelectElement(config.XPath, ns);
                        if (el == null) {
                            throw new ValidationError($"Expected element at '{config.XPath}'", data);
                        }
                        return config.ResponseFactory(el);
                    });
                }

                result.Success = result.Error == null &&
                    result.ResObject != null &&
                    result.ResObject.Odgovor.Greska == null &&
                    result.ResObject.Odgovor.PrihvacenZahtjev;
            }
            catch (Exception ex) {
                result.Error = ErrorUtils.ParseError(ex);
            }

            return await Task.FromResult(result);
        }

        public Task<FiskalizacijaResult<TReq, TRes>> SendAsync<TReqData, TReq, TRes>(
            TReqData requestData,
            RequestConfig<TReqData, TReq, TRes> config
        ) where TReq : SerializableRequest where TRes : ParsedResponse {
            return Execute(requestData!, config);
        }

        public Task<FiskalizacijaResult<TReq, TRes>> SendAsync<TReqData, TReq, TRes>(
            TReq request,
            RequestConfig<TReqData, TReq, TRes> config
        ) where TReq : SerializableRequest where TRes : ParsedResponse {
            return Execute(request!, config);
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
