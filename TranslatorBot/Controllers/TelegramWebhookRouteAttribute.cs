using System;
using Microsoft.AspNetCore.Mvc.Routing;

namespace TranslatorBot.Controllers
{
    public class TelegramWebhookRouteAttribute : Attribute, IRouteTemplateProvider
    {
        private static string _template;

        public string Template => _template;
        public int? Order => 0;
        public string Name => "WebhookRoute";

        public static void SetTemplate(string value)
        {
            if (!string.IsNullOrEmpty(_template))
            {
                throw new InvalidOperationException();
            }
            _template = value;
        }
    }
}