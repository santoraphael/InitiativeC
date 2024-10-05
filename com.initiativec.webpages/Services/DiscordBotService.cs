using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace com.initiativec.webpages.Services
{
    public class DiscordBotService : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger<DiscordBotService> _logger;
        private readonly string _botToken;

        public DiscordBotService(ILogger<DiscordBotService> logger)
        {
            _logger = logger;
            _botToken = ""; // Substitua pelo seu token

            var config = new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildPresences | GatewayIntents.GuildMembers
            };

            _client = new DiscordSocketClient(config);

            _client.Log += LogAsync;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _client.LoginAsync(TokenType.Bot, _botToken);
            await _client.StartAsync();

            // Aguarda até que o serviço seja interrompido
            await Task.Delay(-1, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
            await base.StopAsync(stoppingToken);
        }

        private Task LogAsync(LogMessage log)
        {
            _logger.LogInformation(log.ToString());
            return Task.CompletedTask;
        }

        // Método público para acessar o DiscordSocketClient
        public DiscordSocketClient GetClient()
        {
            return _client;
        }
    }
}
