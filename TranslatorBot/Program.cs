using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TranslatorBot.Models.Options;

namespace TranslatorBot
{
    public static class Program
    {
        private const string DevConfigName = "local-dev.json";
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    foreach (var config in Directory.EnumerateFiles("config", "*.json").Where(name => !name.Contains(DevConfigName)))
                    {
                        builder.AddJsonFile(config, false, true);
                    }
                    builder.AddJsonFile($"config/{DevConfigName}", false, true);
                    builder.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel((context, options) =>
                    {
                        var hostingOptions = context.Configuration.GetSection("HostingOptions").Get<HostingOptions>();
                        var tlsOptions = hostingOptions.Tls;
                        if (tlsOptions.Enabled)
                        {
                            X509Certificate2 certificate;
                            if (!File.Exists(tlsOptions.CertFile) || !File.Exists(tlsOptions.KeyFile))
                            {
                                certificate = CertificateUtils.GenerateAndSaveCertificate(
                                    hostingOptions.BotBaseAddress,
                                    tlsOptions.CertFile,
                                    tlsOptions.KeyFile);
                            }
                            else
                            {
                                var certFromFiles = X509Certificate2.CreateFromPemFile(tlsOptions.CertFile, tlsOptions.KeyFile);
                                certificate = new X509Certificate2(certFromFiles.Export(X509ContentType.Pkcs12));
                            }
                            options.ListenAnyIP(8443, listenOptions =>
                            {
                                listenOptions.UseHttps(certificate, adapterOptions =>
                                {
                                    adapterOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
                                    adapterOptions.CheckCertificateRevocation = false;
                                    adapterOptions.AllowAnyClientCertificate();
                                });
                            });
                        }
                        else
                        {
                            options.ListenAnyIP(8443);
                        }
                    });
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}