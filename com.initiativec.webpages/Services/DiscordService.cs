using com.initiativec.webpages.Interfaces;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace com.initiativec.webpages.Services
{
    public class DiscordService : IDiscordService
    {
        private readonly DiscordBotService _discordBotService;

        public DiscordService(DiscordBotService discordBotService)
        {
            _discordBotService = discordBotService;
        }

        public async Task<bool> IsUserInChannelAsync(ulong userId, ulong guildId, ulong channelId)
        {
            var client = _discordBotService.GetClient();

            // Aguarda até que o cliente esteja pronto
            if (!client.Guilds.Any())
            {
                await Task.Delay(5000); // Aguarda 5 segundos
            }

            var guild = client.GetGuild(guildId);
            if (guild == null)
                return false;

            var channel = guild.GetVoiceChannel(channelId);
            if (channel == null)
                return false;

            var user = guild.GetUser(userId);
            if (user == null)
                return false;

            return user.VoiceChannel != null && user.VoiceChannel.Id == channelId;
        }

    }
}
