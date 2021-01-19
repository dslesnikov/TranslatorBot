using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TranslatorBot.Models.Options;
using Yandex.Cloud.Ai.Translate.V2;

namespace TranslatorBot.Services.Yandex
{
    public class YandexTranslationService : ITranslationService
    {
        private readonly IYandexApiClient _client;
        private readonly IOptionsSnapshot<TranslationOptions> _translationConfig;

        public YandexTranslationService(
            IYandexApiClient client,
            IOptionsSnapshot<TranslationOptions> translationConfig)
        {
            _client = client;
            _translationConfig = translationConfig;
        }

        public async Task<string> TranslateAsync(string text)
        {
            var languages = _translationConfig.Value.Languages;
            var textToTranslate = text;
            var detectLanguageResponse = await _client.DetectLanguageAsync(new DetectLanguageRequest
            {
                Text = text
            });
            var sourceLanguage = detectLanguageResponse.LanguageCode;
            foreach (var targetLanguage in languages.Append(sourceLanguage))
            {
                var request = new TranslateRequest
                {
                    Format = TranslateRequest.Types.Format.PlainText,
                    SourceLanguageCode = sourceLanguage,
                    TargetLanguageCode = targetLanguage,
                    Texts = { textToTranslate }
                };
                var translationResponse = await _client.TranslateAsync(request);
                textToTranslate = translationResponse.Translations[0].Text;
                sourceLanguage = targetLanguage;
            }
            return textToTranslate;
        }
    }
}