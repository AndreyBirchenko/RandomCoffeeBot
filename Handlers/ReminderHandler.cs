using RandomCoffeeBot.DbModels;
using RandomCoffeeBot.Utils;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;
using SlackNet.WebApi;
using DbModels_User = RandomCoffeeBot.DbModels.User;
using RandomCoffeeBot_DbModels_User = RandomCoffeeBot.DbModels.User;
using User = RandomCoffeeBot.DbModels.User;

namespace RandomCoffeeBot.Handlers;

public class ReminderHandler(ISlackApiClient slack) : IBlockActionHandler<ButtonAction>
{
    public const string Yes = "Да";
    public const string No = "Нет";

    public static List<Block> Blocks =>
    [
        new SectionBlock { Text = Messages.ReminderMessage },
        new ActionsBlock
        {
            Elements =
            {
                new SlackNet.Blocks.Button
                {
                    ActionId = Yes,
                    Value = Yes,
                    Text = new PlainText(Yes)
                },
                new SlackNet.Blocks.Button
                {
                    ActionId = No,
                    Value = No,
                    Text = new PlainText(No)
                },
            }
        }
    ];

    public async Task Handle(ButtonAction action, BlockActionRequest request)
    {
        var blocks = request.Message.Blocks;
        blocks.RemoveAt(1);
        var textBlock = blocks[0] as SectionBlock;

        if (action.Value == Yes)
        {
            var user = await Db.GetUserAsync(request.User.Id);
            user!.State = UserState.ReadyForMatch;
            await Db.CreateOrUpdateUserAsync(user);
            textBlock!.Text = Messages.ReminderYesEditedMessage;
            await UpdateActionRequestMessage(request);
            await slack.PostMessageAsync(Messages.ReminderYesResponseMessage, request.Channel.Id);
            return;
        }

        textBlock!.Text = Messages.ReminderNoEditedMessage;
        await UpdateActionRequestMessage(request);
        await slack.PostMessageAsync(Messages.ReminderNoResponseMessage, request.Channel.Id);
    }

    private async Task UpdateActionRequestMessage(BlockActionRequest request)
    {
        await slack.Chat.Update(new MessageUpdate
        {
            Ts = request.Message.Ts,
            Text = request.Message.Text,
            Blocks = request.Message.Blocks,
            ChannelId = request.Channel.Id
        });
    }

    public static async Task RunAsync(ISlackApiClient slack)
    {
        var now = DateTime.Now;
        var nextRun = now.Hour < 12 
            ? new DateTime(now.Year, now.Month, now.Day, 12, 0, 0) 
            : new DateTime(now.Year, now.Month, now.Day, 12, 0, 0).AddDays(1);

        var timeUntilFirstRun = nextRun - now;

        await Task.Delay(timeUntilFirstRun);

        while (true)
        {
            switch (now.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    await SendReminderMessagesAsync(slack);
                    break;
                case DayOfWeek.Monday:
                    await MatchUsersAsync(slack);
                    break;
            }

            await Task.Delay(TimeSpan.FromDays(1));
        }
    }

    public static async Task SendReminderMessagesAsync(ISlackApiClient slack)
    {
        var users = await Db.GetUsersWithStateAsync(UserState.Active);

        foreach (var user in users)
        {
            await slack.Chat.PostMessage(new Message
            {
                Channel = user.Channel,
                Blocks = Blocks
            });
        }
    }

    public static async Task MatchUsersAsync(ISlackApiClient slack)
    {
        var users = await Db.GetUsersWithStateAsync(UserState.ReadyForMatch);
        if (users.Count == 0)
            return;

        var matchesToDb = new List<Match>();
        var processedUsers = new HashSet<RandomCoffeeBot_DbModels_User>();

        foreach (var user in users)
        {
            if (processedUsers.Contains(user))
                continue;

            Log.Message($"Ищем пару для {user.Id}");

            var matchedUsersSet = user.Matches.Select(x => x.MatchedUser).ToHashSet();
            var matchedUser = users.FirstOrDefault(u => u != user
                                                        && !matchedUsersSet.Contains(u)
                                                        && !processedUsers.Contains(u));
            if (matchedUser == null)
            {
                Log.Message($"Не найдена пара для {user.Id}");
                _ = slack.PostMessageAsync(Messages.MatchFailMessage, user.Channel);
                continue;
            }

            var match1 = new Match { UserId = user.Id, MatchedUserId = matchedUser.Id };
            var match2 = new Match { UserId = matchedUser.Id, MatchedUserId = user.Id };
            
            processedUsers.Add(user);
            processedUsers.Add(matchedUser);

            matchesToDb.Add(match1);
            matchesToDb.Add(match2);

            Log.Message($"Создана пара для {user.Id} , {matchedUser.Id}");

            user.State = UserState.Active;
            matchedUser.State = UserState.Active;

            var messageForUser = string.Format(Messages.MatchSuccessMessage, matchedUser.Id, matchedUser.Info);
            _ = slack.PostMessageAsync(messageForUser, user.Channel);
            
            var messageForMatchedUser = string.Format(Messages.MatchSuccessMessage, user.Id, user.Info);
            _ = slack.PostMessageAsync(messageForMatchedUser, matchedUser.Channel);
        }

        await Db.CreateMatchesAsync(matchesToDb);
        await Db.UpdateUsersAsync(users);
    }
}