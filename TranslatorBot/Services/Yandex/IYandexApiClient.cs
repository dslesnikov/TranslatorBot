using Grpc.Core;
using TranslatorBot.Services.Yandex;

namespace TranslatorBot.Services.Yandex
{
    public interface IYandexApiClient
    {
        AsyncUnaryCall<global::Yandex.Cloud.Ai.Translate.V2.TranslateResponse> TranslateAsync(
            global::Yandex.Cloud.Ai.Translate.V2.TranslateRequest request,
            Metadata headers = null,
            System.DateTime? deadline = null,
            System.Threading.CancellationToken cancellationToken = default);

        AsyncUnaryCall<global::Yandex.Cloud.Ai.Translate.V2.DetectLanguageResponse> DetectLanguageAsync(
            global::Yandex.Cloud.Ai.Translate.V2.DetectLanguageRequest request,
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