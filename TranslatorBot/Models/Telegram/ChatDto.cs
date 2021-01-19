namespace TranslatorBot.Models.Telegram
{
    public record ChatDto(
        long Id,
        ChatType Type,
        string Title,
        string Username,
        string FirstName,
        string LastName);
}