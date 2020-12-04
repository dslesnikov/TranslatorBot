namespace TranslatorBot.Models
{
    public record UserDto(
        int Id,
        bool IsBot,
        string FirstName,
        string LastName,
        string Username,
        string LanguageCode);
}