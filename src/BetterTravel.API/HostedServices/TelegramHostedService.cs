using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot;

namespace BetterTravel.API.HostedServices
{
    public class TelegramHostedService : IHostedService
    {
        private readonly TelegramBotClient _client;
        private readonly BotConfiguration _configuration;
        private readonly ILogger _logger;

        public TelegramHostedService(
            TelegramBotClient client,
            BotConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
            _logger = Log.ForContext<TelegramHostedService>();
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _client.DeleteWebhookAsync(cancellationToken);
            _logger.Information("Setting web-hook with {url} {token}", _configuration.WebhookUrl,
                _configuration.BotToken);
            await _client.SetWebhookAsync($"{_configuration.WebhookUrl}/{_configuration.BotToken}",
                cancellationToken: cancellationToken);
            _logger.Information("End setting web-hook");
        }

        public async Task StopAsync(CancellationToken cancellationToken) => 
            await _client.DeleteWebhookAsync(cancellationToken);
    }

    public class BotConfiguration
    {
        public string WebhookUrl { get; set; }
        public string BotToken { get; set; }
    }
}