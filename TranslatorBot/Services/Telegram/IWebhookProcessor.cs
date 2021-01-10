using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TranslatorBot.Services.Telegram
{
    public interface IWebhookProcessor
    {
        Task ProcessRequestAsync(HttpContext context);
    }
}