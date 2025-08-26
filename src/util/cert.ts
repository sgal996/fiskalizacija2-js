import { existsSync, readFileSync } from "node:fs";

export function loadPem(pemOrPath: string | Buffer): string {
    if (Buffer.isBuffer(pemOrPath)) {
        return pemOrPath.toString("utf8");
    }
    if (pemOrPath.includes("-----BEGIN")) {
        return pemOrPath;
    }
    if (existsSync(pemOrPath)) {
        return readFileSync(pemOrPath, "utf8");
    }
    return pemOrPath;
}

export function extractPemCertificate(pem: string | Buffer): string {
    pem = loadPem(pem);
    const match = pem.match(/-----BEGIN CERTIFICATE-----(.*?)-----END CERTIFICATE-----/s);
    if (match && match[0]) {
        return match[0].trim();
    }
    throw new Error("Invalid PEM certificate format");
}

export function extractPemPrivateKey(pem: string | Buffer): string {
    pem = loadPem(pem);
    const match = pem.match(/-----BEGIN (?:RSA )?PRIVATE KEY-----(.*?)-----END (?:RSA )?PRIVATE KEY-----/s);
    if (match && match[0]) {
        return match[0].trim();
    }
    throw new Error("Invalid PEM private key format");
}
