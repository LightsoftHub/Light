using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Telegram
{
    public static class Startup
    {
        /*
         * https://api.telegram.org/bot6377770286:AAHWcpDQ_4G8ycCB6xRd4KITnvNYHvYQPOk/getUpdates
         * Get chat IDs, (only public chanel can be use @userName to send message
         *
         */

        public static IServiceCollection AddTelegram(this IServiceCollection services)
        {
            services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    TelegramBotClientOptions options = new TelegramBotClientOptions("6377770286:AAHWcpDQ_4G8ycCB6xRd4KITnvNYHvYQPOk");
                    return new TelegramBotClient(options, httpClient);
                });

            services.AddTransient<ITelegramService, TelegramBotService>();

            return services;
        }
    }
}