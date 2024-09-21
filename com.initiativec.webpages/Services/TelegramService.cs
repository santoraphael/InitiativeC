using com.initiativec.webpages.ViewModel;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace com.initiativec.webpages.Services
{
    public class TelegramService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly TelegramBotSettings _settings;

        public TelegramService(ITelegramBotClient botClient, IOptions<TelegramBotSettings> options)
        {
            _botClient = botClient;
            _settings = options.Value;
        }

        /// <summary>
        /// Verifica se um usuário está no grupo específico.
        /// </summary>
        /// <param name="userId">ID do usuário no Telegram.</param>
        /// <returns>True se o usuário estiver no grupo, caso contrário, False.</returns>
        public async Task<bool> IsUserInGroupAsync(long userId)
        {
            try
            {
                var member = await _botClient.GetChatMemberAsync(_settings.GroupChatId, userId);
                return member.Status switch
                {
                    ChatMemberStatus.Creator => true,
                    ChatMemberStatus.Administrator => true,
                    ChatMemberStatus.Member => true,
                    _ => false
                };
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) when (ex.ErrorCode == 400)
            {
                // Erro 400: Bad Request. Possivelmente o usuário não está no grupo.
                return false;
            }
            catch (Exception ex)
            {
                // Trate outros tipos de exceções conforme necessário
                Console.WriteLine($"Erro ao verificar participação: {ex.Message}");
                return false;
            }
        }
    }
}
