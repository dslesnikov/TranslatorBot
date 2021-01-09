namespace TranslatorBot.Models.Options
{
    public class TlsOptions
    {
        public bool Enabled { get; init; }
        public string CertFile { get; init; }
        public string KeyFile { get; init; }
    }
}