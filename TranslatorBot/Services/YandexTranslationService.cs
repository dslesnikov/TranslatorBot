using System.Threading.Tasks;
using Yandex.Cloud.Ai.Translate.V2;

namespace TranslatorBot.Services
{
    public class YandexTranslationService : ITranslationService
    {
        private readonly TranslationService.TranslationServiceClient _client;

        public YandexTranslationService(TranslationService.TranslationServiceClient client)
        {
            _client = client;
        }

        public async Task<string> TranslateAsync(string text)
        {
            var request = new TranslateRequest
            {
                Texts = {text},
                SourceLanguageCode = "ru",
                TargetLanguageCode = "en"
            };
            var result = await _client.TranslateAsync(request);
            return result.Translations[0].Text;
        }
    }
}