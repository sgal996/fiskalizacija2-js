using System;

namespace Fiskalizacija2 {
    public class XmlSigner {
        public XmlSigner(FiskalizacijaOptions options) {
            // TODO: load certificates and prepare signing configuration
        }

        public string SignFiscalizationRequest(string xml, string id) {
            // TODO: implement signing of XML payload
            return xml;
        }

        public static bool IsValidSignature(string xml) {
            // TODO: implement signature validation
            return true;
        }
    }
}
