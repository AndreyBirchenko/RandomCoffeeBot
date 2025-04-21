using RandomCoffeeBot.Utils;

namespace RandomCoffeeBot.DbModels;

public class User
{
    public string Id { get; set; }
    public string Info { get; set; }
    public UserState State { get; set; }
    public string Channel { get; set; }
    
    public List<Match> Matches { get; set; } = new();
}