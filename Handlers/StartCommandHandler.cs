using RandomCoffeeBot.Utils;
using SlackNet.Interaction;
using DbModels_User = RandomCoffeeBot.DbModels.User;
using RandomCoffeeBot_DbModels_User = RandomCoffeeBot.DbModels.User;
using User = RandomCoffeeBot.DbModels.User;

namespace RandomCoffeeBot.Handlers;

public class StartCommandHandler : ISlashCommandHandler
{
    public const string Command = "/start";

    public async Task<SlashCommandResponse> Handle(SlashCommand command)
    {
        var userId = command.UserId;
        var user = await Db.GetUserAsync(userId);

        if (user is null)
        {
            user = new RandomCoffeeBot_DbModels_User 
            { 
                Id = userId, 
                Info = "", 
                State = UserState.WaitingForInfo,
                Channel = command.ChannelId
            };
            await Db.CreateOrUpdateUserAsync(user);
            return new SlashCommandTextResponse(Messages.StartMessage);
        }
        
        if(user.State == UserState.Paused)
        {
            user.State = UserState.Active;
            await Db.CreateOrUpdateUserAsync(user);
            return new SlashCommandTextResponse(Messages.ResumeMeetingsMessage);
        }

        return new SlashCommandResponse();
    }
}