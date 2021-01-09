using Grpc.Core;
using TranslatorBot.Services;

namespace TranslatorBot.Services
{
    public interface IYandexApiClient
    {
        AsyncUnaryCall<Yandex.Cloud.Ai.Translate.V2.TranslateResponse> TranslateAsync(
            Yandex.Cloud.Ai.Translate.V2.TranslateRequest request,
            Metadata headers = null,
            System.DateTime? deadline = null,
            System.Threading.CancellationToken cancellationToken = default);

        AsyncUnaryCall<Yandex.Cloud.Ai.Translate.V2.DetectLanguageResponse> DetectLanguageAsync(
            Yandex.Cloud.Ai.Translate.V2.DetectLanguageRequest request,
            Metadata headers = null,
            System.DateTime? deadline = null,
            System.Threading.CancellationToken cancellationToken = default);
    }
}

namespace Yandex.Cloud.Ai.Translate.V2
{
    public partial class TranslationService
    {
        public partial class TranslationServiceClient : IYandexApiClient
        {
        }
    }
}