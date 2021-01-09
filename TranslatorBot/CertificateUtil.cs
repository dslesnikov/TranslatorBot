using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace TranslatorBot
{
    public static class CertificateUtil
    {
        public static X509Certificate2 GenerateAndSaveCertificate(string commonName, string certPath, string keyPath)
        {
            var rsaKey = RSA.Create(4096);
            var req = new CertificateRequest($"cn={commonName}", rsaKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            using var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(365));
            var publicKey = cert.Export(X509ContentType.Cert);
            var publicKeyText = $@"
-----BEGIN CERTIFICATE-----
{Convert.ToBase64String(publicKey, Base64FormattingOptions.InsertLineBreaks)}
-----END CERTIFICATE-----
";
            File.WriteAllText(certPath, publicKeyText.Trim());
            var privateKey = rsaKey.ExportRSAPrivateKey();
            var privateKeyText = $@"
-----BEGIN RSA PRIVATE KEY-----
{Convert.ToBase64String(privateKey, Base64FormattingOptions.InsertLineBreaks)}
-----END RSA PRIVATE KEY-----
";
            File.WriteAllText(keyPath, privateKeyText.Trim());
            return new X509Certificate2(cert.Export(X509ContentType.Pkcs12));
        }
    }
}