namespace TranslatorBot.Models.Options
{
    public class HostingOptions
    {
        public string BotBaseAddress { get; init; }

        public int BotPort { get; init; }
        
        public TlsOptions Tls { get; init; }
    }
}