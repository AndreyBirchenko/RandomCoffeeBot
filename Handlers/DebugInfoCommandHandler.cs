using RandomCoffeeBot.Utils;
using SlackNet.Interaction;

namespace RandomCoffeeBot.Handlers;

public class DebugInfoCommandHandler : ISlashCommandHandler
{
    public const string Command = "/debug_info";

    public async Task<SlashCommandResponse> Handle(SlashCommand command)
    {
        var users = await Db.GetUsersAsync();

        var allUsersCount = users?.Count ?? 0;
        var activeUsersCount = users?.Where(x => x.State == UserState.Active).Count() ?? 0;
        var pausedUsersCount = users?.Where(x => x.State == UserState.Paused).Count() ?? 0;

        var message = string.Format(Messages.DebugInfoMessage, allUsersCount, activeUsersCount, pausedUsersCount);
        return new SlashCommandTextResponse(message);
    }
}