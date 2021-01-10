using System.Threading.Tasks;

namespace TranslatorBot.Services.Telegram
{
    public interface ITelegramApiService
    {
        Task SetWebhookAsync();

        Task SendMessageAsync(int chatId, string text, int replyToId);
    }
}