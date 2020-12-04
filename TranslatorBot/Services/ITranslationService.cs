using System.Threading.Tasks;

namespace TranslatorBot.Services
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text);
    }
}