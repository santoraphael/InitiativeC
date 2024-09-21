using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace com.initiativec.webpages.Services
{
    public class BotHostedService : IHostedService
    {
        private readonly BotService _botService;
        private readonly ITelegramBotClient _botClient;

        public BotHostedService(BotService botService, ITelegramBotClient botClient)
        {
            _botService = botService;
            _botClient = botClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _botClient.StartReceiving(_botService,
                new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>() // Recebe todos os tipos de atualizações
                },
                cancellationToken
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //_botClient.StopReceiving(cancellationToken);
            return Task.CompletedTask;
        }
    }
}
