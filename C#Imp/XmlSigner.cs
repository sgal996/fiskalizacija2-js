using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Fiskalizacija2 {
    public class XmlSigner {
        private readonly X509Certificate2 _certificate;

        public XmlSigner(FiskalizacijaOptions options) {
            if (string.IsNullOrWhiteSpace(options.PrivateKey) || string.IsNullOrWhiteSpace(options.PublicCert)) {
                throw new ArgumentException("Private key and public certificate are required", nameof(options));
            }

            using var publicCert = X509Certificate2.CreateFromPem(options.PublicCert);
            using var rsa = RSA.Create();
            rsa.ImportFromPem(options.PrivateKey);
            _certificate = publicCert.CopyWithPrivateKey(rsa);
        }

        public string SignFiscalizationRequest(string xml, string id) {
            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.LoadXml(xml);

            var signedXml = new SignedXml(doc) {
                SigningKey = _certificate.GetRSAPrivateKey()
            };

            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;

            var reference = new Reference { Uri = "#" + id, DigestMethod = SignedXml.XmlDsigSHA256Url };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference);

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(_certificate));
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();
            var xmlSignature = signedXml.GetXml();
            doc.DocumentElement?.AppendChild(doc.ImportNode(xmlSignature, true));
            return doc.OuterXml;
        }

        public static bool IsValidSignature(string xml) {
            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.LoadXml(xml);

            var nodeList = doc.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl);
            if (nodeList.Count == 0) {
                throw new Exception("No Signature element found");
            }

            var signedXml = new SignedXml(doc);
            signedXml.LoadXml((XmlElement)nodeList[0]);

            X509Certificate2? certificate = null;
            foreach (KeyInfoClause clause in signedXml.KeyInfo) {
                if (clause is KeyInfoX509Data x509Data && x509Data.Certificates.Count > 0) {
                    var cert = x509Data.Certificates[0] as X509Certificate;
                    if (cert != null) {
                        certificate = new X509Certificate2(cert);
                        break;
                    }
                }
            }

            if (certificate == null) {
                throw new Exception("No certificate found in signature");
            }

            return signedXml.CheckSignature(certificate, true);
        }
    }
}
