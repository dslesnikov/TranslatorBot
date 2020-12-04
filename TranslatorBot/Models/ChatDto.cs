namespace TranslatorBot.Models
{
    public record ChatDto(
        int Id,
        string Type,
        string Title,
        string Username,
        string FirstName,
        string LastName);
}