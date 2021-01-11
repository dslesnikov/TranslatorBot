namespace TranslatorBot.Models.Telegram
{
    public record ChatDto(
        long Id,
        string Type,
        string Title,
        string Username,
        string FirstName,
        string LastName);
}