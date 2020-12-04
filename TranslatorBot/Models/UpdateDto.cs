namespace TranslatorBot.Models
{
    public record UpdateDto(
        int UpdateId,
        MessageDto Message,
        MessageDto EditedMessage,
        MessageDto ChannelPost,
        MessageDto EditChannelPost
    );
}