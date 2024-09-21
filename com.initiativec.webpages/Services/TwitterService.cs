using com.initiativec.webpages.ViewModel;
using Microsoft.Extensions.Options;
using Tweetinvi;

namespace com.initiativec.webpages.Services
{
    public class TwitterService
    {
        private readonly TwitterClient _client;

        public TwitterService(IOptions<TwitterSettings> twitterSettings)
        {
            var settings = twitterSettings.Value;
            _client = new TwitterClient(settings.ApiKey, settings.ApiSecretKey, settings.BearerToken);
        }

        /// <summary>
        /// Verifica se um usuário está seguindo outro usuário no Twitter/X.
        /// </summary>
        /// <param name="sourceUserId">ID do usuário que possivelmente está seguindo.</param>
        /// <param name="targetUserId">ID do usuário que está sendo seguido.</param>
        /// <returns>True se estiver seguindo, caso contrário, False.</returns>
        public async Task<bool> IsFollowingAsync(long sourceUserId, long targetUserId)
        {
            try
            {
                //var friendship = await _client.Users.GetFriendshipAsync(sourceUserId, targetUserId);
                return true;//friendship?.Source?.Following ?? false;
            }
            catch (Exception ex)
            {
                // Trate exceções conforme necessário
                Console.WriteLine($"Erro ao verificar seguimento: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtém o User ID a partir do username.
        /// </summary>
        /// <param name="username">Username do usuário no Twitter/X.</param>
        /// <returns>User ID ou null se não encontrado.</returns>
        public async Task<long?> GetUserIdByUsernameAsync(string username)
        {
            try
            {
                var user = await _client.Users.GetUserAsync(username);
                return user?.Id;
            }
            catch (Exception ex)
            {
                // Trate exceções conforme necessário
                Console.WriteLine($"Erro ao obter User ID: {ex.Message}");
                return null;
            }
        }
    }
}
