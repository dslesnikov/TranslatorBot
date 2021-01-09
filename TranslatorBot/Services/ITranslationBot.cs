using System.Threading.Tasks;
using TranslatorBot.Models.Telegram;

namespace TranslatorBot.Services
{
    public interface ITranslationBot
    {
        Task ProcessUpdate(UpdateDto dto);
    }
}