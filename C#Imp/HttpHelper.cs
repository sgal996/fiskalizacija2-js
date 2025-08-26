using System;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Fiskalizacija2 {
    public static class HttpHelper {
        public static async Task<(int statusCode, string data)> PostRequestAsync(string data, FiskalizacijaOptions options) {
            var handler = new HttpClientHandler();

            if (options.Ca != null && options.Ca.Length > 0) {
                var caCert = new X509Certificate2(options.Ca);
                handler.ServerCertificateCustomValidationCallback = (req, cert, chain, errors) => {
                    chain.ChainPolicy.CustomTrustStore.Add(caCert);
                    chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
                    return chain.Build(cert);
                };
            }

            if (options.AllowSelfSigned) {
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }
            using var client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromMilliseconds(options.Timeout);
            var request = new HttpRequestMessage(HttpMethod.Post, options.Service);
            foreach (var header in options.Headers) {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            request.Content = new StringContent(data, Encoding.UTF8, "application/xml");
            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            return ((int)response.StatusCode, body);
        }
    }
}
