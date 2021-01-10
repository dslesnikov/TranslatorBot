using System.Threading.Tasks;
using TranslatorBot.Models.Telegram;

namespace TranslatorBot.Services.Telegram
{
    public interface ITranslationBot
    {
        Task ProcessUpdateAsync(UpdateDto dto);
    }
}