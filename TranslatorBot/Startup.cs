using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TranslatorBot.Controllers;
using TranslatorBot.Services;
using Yandex.Cloud.Ai.Translate.V2;

namespace TranslatorBot
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var token = _configuration["TelegramOptions:BotToken"];
            TelegramWebhookRouteAttribute.SetTemplate($"/{token}");
            services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                });
            services.AddGrpcClient<TranslationService.TranslationServiceClient>(opts =>
            {
                opts.Address = new Uri(_configuration["YandexCloudOptions:TranslationEndpoint"]);
            });
            services.AddScoped<ITranslationService, YandexTranslationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}