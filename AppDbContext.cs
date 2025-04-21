using Microsoft.EntityFrameworkCore;
using RandomCoffeeBot.DbModels;

namespace RandomCoffeeBot;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Match> Matches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source=app.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Match>()
            .HasOne(m => m.User)
            .WithMany(u => u.Matches)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.MatchedUser)
            .WithMany()
            .HasForeignKey(m => m.MatchedUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}