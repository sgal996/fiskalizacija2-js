using System;
using System.Collections.Generic;

namespace Fiskalizacija2 {
    public class ErrorWithMessage {
        public string Message { get; set; } = string.Empty;
        public object? Thrown { get; set; }
    }

    public class SigningOptions {
        public string PrivateKey { get; set; } = string.Empty;
        public string PublicCert { get; set; } = string.Empty;
        public string? SignatureAlgorithm { get; set; }
        public string? CanonicalizationAlgorithm { get; set; }
        public string? DigestAlgorithm { get; set; }
    }

    public class FiskalizacijaOptions : SigningOptions {
        public byte[]? Ca { get; set; }
        public string Service { get; set; } = string.Empty;
        public bool AllowSelfSigned { get; set; }

    public class FiskalizacijaOptions {

        public int Timeout { get; set; } = 30000;
        public Dictionary<string, string> Headers { get; set; } = new();
    }

    public interface IGreska {
        string? Poruka { get; }
    }

    public class FiskalizacijaResult<TReq, TRes> {
        public bool Success { get; set; }

        public ErrorWithMessage? Error { get; set; }
        public int? HttpStatusCode { get; set; }
        public string? SoapReqRaw { get; set; }
        public TReq? ReqObject { get; set; }
        public string? SoapResRaw { get; set; }
        public bool? SoapResSignatureValid { get; set; }
        public TRes? ResObject { get; set; }

        public TReq? ReqObject { get; set; }
        public TRes? ResObject { get; set; }
        public int? HttpStatusCode { get; set; }
        public bool? SoapResSignatureValid { get; set; }
        public string? SoapReqRaw { get; set; }
        public string? SoapResRaw { get; set; }
        public Exception? Error { get; set; }

    }

    public interface SerializableRequest {
        string ToXmlString();
        string Id { get; }
    }

    public interface ParsedResponse {
        OdgovorContent Odgovor { get; }
    }


    public class OdgovorContent {
        public bool PrihvacenZahtjev { get; set; }
        public IGreska? Greska { get; set; }
    }


    public interface ParsedResponse {
        OdgovorContent Odgovor { get; }
    }

    public class RequestConfig<TReqData, TReq, TRes>
        where TReq : SerializableRequest
        where TRes : ParsedResponse {
        public Func<TReqData, TReq>? RequestFactory { get; set; }
        public Func<string, TRes>? ResponseFactory { get; set; }

    public class RequestConfig<TReqData, TReq, TRes>
        where TReq : SerializableRequest
        where TRes : ParsedResponse {
        public Func<TReqData, TReq> RequestFactory { get; set; } = default!;
        public Func<string, TRes> ResponseFactory { get; set; } = default!;

        public string XPath { get; set; } = string.Empty;
    }
}
