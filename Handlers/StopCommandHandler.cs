using RandomCoffeeBot.Utils;
using SlackNet.Interaction;

namespace RandomCoffeeBot.Handlers;

public class StopCommandHandler : ISlashCommandHandler
{
    public const string Command = "/stop";

    public async Task<SlashCommandResponse> Handle(SlashCommand command)
    {
        var userId = command.UserId;
        var user = await Db.GetUserAsync(userId);
        
        if (user is null)
        {
            return new SlashCommandTextResponse(Messages.InvalidMessage);
        }
        
        user.State = UserState.Paused;
        await Db.CreateOrUpdateUserAsync(user);
        return new SlashCommandTextResponse(Messages.PauseMessage);
    }
}