using SlackNet.Interaction;
using SlackNet.WebApi;

namespace RandomCoffeeBot.Utils;

public class SlashCommandTextResponse : SlashCommandResponse
{
    public SlashCommandTextResponse(string text)
    {
        Message = new Message { Text = text };
    }
}