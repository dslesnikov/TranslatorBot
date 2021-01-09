using System;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TranslatorBot.Models.Options;
using TranslatorBot.Models.Telegram;
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
            services.Configure<TelegramOptions>(_configuration.GetSection("TelegramOptions"));
            services.Configure<TranslationOptions>(_configuration.GetSection("TranslationOptions"));
            services.Configure<HostingOptions>(_configuration.GetSection("HostingOptions"));
            services.AddScoped<ITranslationService, YandexTranslationService>();
            services.AddScoped<ITranslationBot, TranslationBot>();
            services.AddScoped<ITelegramApiService, TelegramApiService>();
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
                endpoints.MapPost($"/{options.Value.BotToken}", static async context =>
                {
                    var update = await JsonSerializer.DeserializeAsync<UpdateDto>(context.Request.Body,
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = SnakeCaseNamingPolicy.Policy
                        });
                    var bot = context.RequestServices.GetRequiredService<ITranslationBot>();
                    await bot.ProcessUpdate(update);
                    context.Response.StatusCode = 200;
                });
            });

            using var scope = app.ApplicationServices.CreateScope();
            var sp = scope.ServiceProvider;
            var service = sp.GetRequiredService<ITelegramApiService>();
            service.SetWebhook().GetAwaiter().GetResult();
        }
    }
}