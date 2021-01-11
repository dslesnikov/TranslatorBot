using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TranslatorBot.Models.Options;
using TranslatorBot.Models.Telegram;

namespace TranslatorBot.Services.Telegram
{
    public class TranslationBot : ITranslationBot
    {
        private readonly ITranslationService _translationService;
        private readonly ITelegramApiService _telegramApi;
        private readonly IOptions<TelegramOptions> _telegramOptions;
        private string BotId => _telegramOptions.Value.BotId;

        public TranslationBot(
            ITranslationService translationService,
            ITelegramApiService telegramApi,
            IOptions<TelegramOptions> telegramOptions)
        {
            _translationService = translationService;
            _telegramApi = telegramApi;
            _telegramOptions = telegramOptions;
        }

        public async Task ProcessUpdateAsync(UpdateDto dto)
        {
            var text = dto.Message.Chat.Type switch
            {
                "private" => dto.Message.ReplyToMessage != null
                    ? dto.Message.ReplyToMessage.Text
                    : dto.Message.Text,
                "group" or "supergroup" when dto.Message.Text?.Contains(BotId) ?? false =>
                    dto.Message.ReplyToMessage != null
                        ? dto.Message.ReplyToMessage.Text
                        : dto.Message.Text,
                _ => null
            };
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            text = text.Replace(BotId, string.Empty).Trim();
            if (!string.IsNullOrEmpty(text) && text.Length <= 1000)
            {
                var translated = await _translationService.TranslateAsync(text);
                await _telegramApi.SendMessageAsync(dto.Message.Chat.Id, translated, dto.Message.MessageId);
            }
        }
    }
}