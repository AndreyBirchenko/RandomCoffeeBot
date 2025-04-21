using RandomCoffeeBot.Utils;
using SlackNet.Interaction;

namespace RandomCoffeeBot.Handlers;

public class HelpCommandHandler : ISlashCommandHandler
{
    public const string Command = "/help";

    public Task<SlashCommandResponse> Handle(SlashCommand command)
    {
        return Task.FromResult<SlashCommandResponse>(new SlashCommandTextResponse(Messages.CommandsMessage));
    }
}