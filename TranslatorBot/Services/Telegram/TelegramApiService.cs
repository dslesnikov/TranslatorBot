using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TranslatorBot.Models.Options;

namespace TranslatorBot.Services.Telegram
{
    public class TelegramApiService : ITelegramApiService
    {
        private readonly IOptions<TelegramOptions> _telegramOptions;
        private readonly IOptions<HostingOptions> _hostingOptions;
        private readonly HttpClient _telegramApiClient;

        public TelegramApiService(
            HttpClient telegramApiClient,
            IOptions<TelegramOptions> telegramOptions,
            IOptions<HostingOptions> hostingOptions)
        {
            _telegramApiClient = telegramApiClient;
            _telegramOptions = telegramOptions;
            _hostingOptions = hostingOptions;
        }

        public async Task SetWebhookAsync()
        {
            var telegramOptions = _telegramOptions.Value;
            var botToken = telegramOptions.BotToken;
            var hostingOptions = _hostingOptions.Value;
            await using var publicKey = File.OpenRead(hostingOptions.Tls.CertFile);
            var content = new MultipartFormDataContent
            {
                { new StreamContent(publicKey), "certificate", "certificate" },
                { new StringContent($"https://{hostingOptions.BotBaseAddress}:{hostingOptions.BotPort}/{botToken}"), "url" }
            };
            var response = await _telegramApiClient.PostAsync("setWebhook", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendMessageAsync(int chatId, string text, int replyToId)
        {
            var content = JsonContent.Create(new
            {
                chat_id = chatId,
                text = text,
                reply_to_message_id = replyToId
            });
            var response = await _telegramApiClient.PostAsync("sendMessage", content);
            response.EnsureSuccessStatusCode();
        }
    }
}