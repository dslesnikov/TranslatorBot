using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TranslatorBot.Models.Options;
using TranslatorBot.Services;
using TranslatorBot.Services.Telegram;
using TranslatorBot.Services.Yandex;
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
            ConfigureOptions(services);

            services.AddGrpcClient<IYandexApiClient>(opts =>
            {
                opts.Creator = invoker => new TranslationService.TranslationServiceClient(invoker);
                var yandexCloudOptions = _configuration.GetSection("YandexCloudOptions").Get<YandexCloudOptions>();
                opts.Address = new Uri(yandexCloudOptions.TranslationEndpoint);
                opts.ChannelOptionsActions.Add(channelOptions =>
                {
                    channelOptions.HttpClient!.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Api-Key", yandexCloudOptions.ApiToken);
                });
            });

            services.AddScoped<ITranslationService, YandexTranslationService>();
            services.AddScoped<ITelegramApiService, TelegramApiService>();
            services.AddScoped<ITranslationBot, TranslationBot>();
            services.AddSingleton<IWebhookProcessor, WebhookProcessor>();

            services.AddHttpClient<ITelegramApiService, TelegramApiService>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<TelegramOptions>>();
                client.BaseAddress = new Uri($"{options.Value.TelegramApiBaseUrl}/bot{options.Value.BotToken}/");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                var options = app.ApplicationServices.GetRequiredService<IOptions<TelegramOptions>>();
                var requestProcessor = app.ApplicationServices.GetRequiredService<IWebhookProcessor>();
                endpoints.MapPost($"/{options.Value.BotToken}", requestProcessor.ProcessRequestAsync);
            });

            InitializeWebhookAsync(app.ApplicationServices).GetAwaiter().GetResult();
        }

        private async Task InitializeWebhookAsync(IServiceProvider rootProvider)
        {
            using var scope = rootProvider.CreateScope();
            var sp = scope.ServiceProvider;
            var service = sp.GetRequiredService<ITelegramApiService>();
            await service.SetWebhookAsync();
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<TelegramOptions>(_configuration.GetSection("TelegramOptions"));
            services.Configure<TranslationOptions>(_configuration.GetSection("TranslationOptions"));
            services.Configure<HostingOptions>(_configuration.GetSection("HostingOptions"));
        }
    }
}