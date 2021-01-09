using System.Threading.Tasks;

namespace TranslatorBot.Services
{
    public interface ITelegramApiService
    {
        Task SetWebhook();

        Task SendMessage(int chatId, string text, int replyToId);
    }
}