namespace RandomCoffeeBot.DbModels;

public class Match
{
    public int Id { get; set; }  
    public string UserId { get; set; }  
    public string MatchedUserId { get; set; }  

    public User User { get; set; }  
    public User MatchedUser { get; set; }  
}
