namespace RandomCoffeeBot.Models;

record AppSettings
{
    public string ApiToken { get; init; } = string.Empty;
    public string AppLevelToken { get; init; } = string.Empty;
}