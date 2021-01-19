namespace TranslatorBot.Models.Telegram
{
    public record MessageEntityDto(
        MessageEntityType Type,
        int Offset,
        int Length,
        string Url,
        UserDto User,
        string Language);
}