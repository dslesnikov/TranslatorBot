namespace TranslatorBot.Models.Telegram
{
    public record MessageDto(
        long MessageId,
        UserDto From,
        ChatDto SenderChat,
        int Date,
        ChatDto  Chat,
        UserDto ForwardFrom,
        ChatDto ForwardFromChat,
        int ForwardFromMessageId,
        string ForwardSignature,
        string ForwardSenderName,
        int ForwardDate,
        MessageDto ReplyToMessage,
        UserDto ViaBot,
        int EditDate,
        string MediaGroupId,
        string AuthorSignature,
        string Text,
        MessageEntityDto[] Entities
    );
}