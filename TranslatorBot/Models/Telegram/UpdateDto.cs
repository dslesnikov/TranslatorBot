namespace TranslatorBot.Models.Telegram
{
    public record UpdateDto(
        long UpdateId,
        MessageDto Message,
        MessageDto EditedMessage,
        MessageDto ChannelPost,
        MessageDto EditChannelPost
    );
}