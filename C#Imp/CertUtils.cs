using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Fiskalizacija2.Utils
{
    public static class CertUtils
    {
        private static readonly Regex CertRegex = new Regex("-----BEGIN CERTIFICATE-----(.*?)-----END CERTIFICATE-----", RegexOptions.Singleline);
        private static readonly Regex KeyRegex = new Regex("-----BEGIN (?:RSA )?PRIVATE KEY-----(.*?)-----END (?:RSA )?PRIVATE KEY-----", RegexOptions.Singleline);

        public static string ExtractPemCertificate(string pem)
        {
            var match = CertRegex.Match(pem);
            if (match.Success)
            {
                return match.Value.Trim();
            }
            throw new ArgumentException("Invalid PEM certificate format");
        }

        public static string ExtractPemCertificate(byte[] pem)
        {
            return ExtractPemCertificate(Encoding.UTF8.GetString(pem));
        }

        public static string ExtractPemPrivateKey(string pem)
        {
            var match = KeyRegex.Match(pem);
            if (match.Success)
            {
                return match.Value.Trim();
            }
            throw new ArgumentException("Invalid PEM private key format");
        }

        public static string ExtractPemPrivateKey(byte[] pem)
        {
            return ExtractPemPrivateKey(Encoding.UTF8.GetString(pem));
        }
    }
}
