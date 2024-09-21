using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace com.initiativec.webpages.Services
{
    public class BotService : IUpdateHandler
    {
        private readonly ILogger<BotService> _logger;
        private readonly ITelegramBotClient _botClient;

        public BotService(ILogger<BotService> logger, ITelegramBotClient botClient)
        {
            _logger = logger;
            _botClient = botClient;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
            {
                var chatId = update.Message.Chat.Id;
                var userId = update.Message.From.Id;
                var username = update.Message.From.Username;

                // Aqui, você pode salvar o userId e associá-lo ao seu sistema de usuários
                // Por exemplo, salve no banco de dados com a referência ao usuário do sistema

                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Olá @{username}, seu ID do Telegram é {userId}.",
                    cancellationToken: cancellationToken
                );
            }
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Erro no bot");
            return Task.CompletedTask;
        }
    }
}
