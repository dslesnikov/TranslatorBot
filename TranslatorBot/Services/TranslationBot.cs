using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TranslatorBot.Models.Options;
using TranslatorBot.Models.Telegram;

namespace TranslatorBot.Services
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

        public async Task ProcessUpdate(UpdateDto dto)
        {
            string text = null;
            if (dto.Message.Chat.Type == "private" ||
                dto.Message.Chat.Type == "group" && (dto.Message.Text?.Contains(BotId) ?? false))
            {
                text = dto.Message.ReplyToMessage != null
                    ? dto.Message.ReplyToMessage.Text
                    : dto.Message.Text;
            }
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            text = text.Replace(BotId, string.Empty).Trim();
            if (!string.IsNullOrEmpty(text) && text.Length <= 500)
            {
                var translated = await _translationService.TranslateAsync(text);
                await _telegramApi.SendMessage(dto.Message.Chat.Id, translated, dto.Message.MessageId);
            }
        }
    }
}