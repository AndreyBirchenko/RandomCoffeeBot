using RandomCoffeeBot.Utils;
using SlackNet;
using SlackNet.Events;

namespace RandomCoffeeBot.Handlers;

class TextMessageHandler(ISlackApiClient slack) : IEventHandler<MessageEvent>
{
    public async Task Handle(MessageEvent slackEvent)
    {
        var testResponse = await slack.Auth.Test();
        if(testResponse.UserId == slackEvent.User) // сообщение от бота
            return;
        
        var text = slackEvent.Text?.Trim();
        if (string.IsNullOrWhiteSpace(text))
            return;
        
        var userInfo = await slack.Users.Info(slackEvent.User);

        var user = await Db.GetUserAsync(userInfo.Id);
        var state = user?.State ?? UserState.None;

        if (state == UserState.WaitingForInfo)
        {
            var infoExists = !string.IsNullOrEmpty(user!.Info);
            
            user.State = UserState.Active;
            user.Info = text;
            await Db.CreateOrUpdateUserAsync(user);

            var message = infoExists ? "Готово" : Messages.InfoReceivedMessage;
            await slack.PostMessageAsync(message, slackEvent.Channel);
            return;
        }

        await slack.PostMessageAsync(Messages.DontUnderstandMessage, slackEvent.Channel);
    }
}