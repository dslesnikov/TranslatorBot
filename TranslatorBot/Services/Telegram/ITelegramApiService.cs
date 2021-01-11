using System.Threading.Tasks;

namespace TranslatorBot.Services.Telegram
{
    public interface ITelegramApiService
    {
        Task SetWebhookAsync();

        Task SendMessageAsync(long chatId, string text, long replyToId);
    }
}