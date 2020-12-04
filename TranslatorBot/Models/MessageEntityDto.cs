namespace TranslatorBot.Models
{
    public record MessageEntityDto(
        string Type,
        int Offset,
        int Length,
        string Url,
        UserDto User,
        string Language);
}