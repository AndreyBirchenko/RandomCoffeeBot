using RandomCoffeeBot.Utils;
using SlackNet.Interaction;

namespace RandomCoffeeBot.Handlers;

public class ShowInfoCommandHandler : ISlashCommandHandler
{
    public const string Command = "/show_info";

    public async Task<SlashCommandResponse> Handle(SlashCommand command)
    {
        var userId = command.UserId;
        var user = await Db.GetUserAsync(userId);
        
        return user is null 
            ? new SlashCommandTextResponse(Messages.InvalidMessage) 
            : new SlashCommandTextResponse(user.Info);
    }
}