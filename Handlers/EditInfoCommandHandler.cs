using RandomCoffeeBot.Utils;
using SlackNet.Interaction;

namespace RandomCoffeeBot.Handlers;

public class EditInfoCommandHandler : ISlashCommandHandler
{
    public const string Command = "/edit_info";

    public async Task<SlashCommandResponse> Handle(SlashCommand command)
    {
        var userId = command.UserId;
        var user = await Db.GetUserAsync(userId);
        
        if (user is null)
        {
            return new SlashCommandTextResponse(Messages.InvalidMessage);
        }

        user!.State = UserState.WaitingForInfo;
        await Db.CreateOrUpdateUserAsync(user);
        
        return new SlashCommandTextResponse(Messages.EditInfoMessage);
    }
}