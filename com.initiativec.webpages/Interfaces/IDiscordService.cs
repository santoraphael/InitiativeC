using System.Threading.Tasks;


namespace com.initiativec.webpages.Interfaces
{
    public interface IDiscordService
    {
        Task<bool> IsUserInChannelAsync(ulong userId, ulong guildId, ulong channelId);
    }
}
