using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TranslatorBot.Json;
using TranslatorBot.Models.Telegram;

namespace TranslatorBot.Services.Telegram
{
    public class WebhookProcessor : IWebhookProcessor
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Policy,
            PropertyNameCaseInsensitive = true,
            Converters = { new PolicyBasedJsonEnumConverter(SnakeCaseNamingPolicy.Policy) }
        };
        
        public async Task ProcessRequestAsync(HttpContext context)
        {
            var update = await context.Request.ReadFromJsonAsync<UpdateDto>(SerializerOptions);
            var bot = context.RequestServices.GetRequiredService<ITranslationBot>();
            await bot.ProcessUpdateAsync(update);
            context.Response.StatusCode = 200;
        }
    }
}