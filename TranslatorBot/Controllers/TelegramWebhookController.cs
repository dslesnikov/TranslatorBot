using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TranslatorBot.Models;
using TranslatorBot.Services;

namespace TranslatorBot.Controllers
{
    public class TelegramWebhookController : ControllerBase
    {
        private readonly ITranslationService _service;

        public TelegramWebhookController(ITranslationService service)
        {
            _service = service;
        }

        [TelegramWebhookRoute]
        public async Task<IActionResult> GetUpdates(UpdateDto update)
        {
            return Ok();
        }
    }
}