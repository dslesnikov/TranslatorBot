using System.Linq;
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
            string text = null;
            switch (dto.Message.Chat.Type)
            {
                case ChatType.Private:
                    text = dto.Message.Text;
                    break;
                case ChatType.Group:
                case ChatType.SuperGroup:
                    var messageText = dto.Message.Text;
                    var messageMentionsBot = dto.Message.Entities != null &&
                                             dto.Message.Entities.Any(e =>
                                                 e.Type == MessageEntityType.Mention &&
                                                 messageText.Substring(e.Offset, e.Length) == BotId);
                    if (messageMentionsBot)
                    {
                        text = dto.Message.ReplyToMessage?.Text ?? dto.Message.Text;
                    }
                    break;
            }
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