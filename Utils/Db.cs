using Microsoft.EntityFrameworkCore;
using RandomCoffeeBot.DbModels;

namespace RandomCoffeeBot.Utils;

public static class Db
{
    public static async Task<User?> GetUserAsync(string userId)
    {
        try
        {
            await using var db = new AppDbContext();
            return await db.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }
        catch (Exception ex)
        {
            Log.Message($"Ошибка при получении пользователя {userId} : {ex.Message}");
            return null;
        }
    }

    public static async Task<bool> CreateOrUpdateUserAsync(User data)
    {
        try
        {
            await using var db = new AppDbContext();
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (user == null)
            {
                await db.Users.AddAsync(data);
                await db.SaveChangesAsync();
                Log.Message($"Создан пользователь Имя: Id: {data.Id}");
                return true;
            }

            user.State = data.State;
            user.Info = data.Info;
            await db.SaveChangesAsync();
            
            Log.Message($"Обновлён пользователь Id: {data.Id}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении пользователя {data.Id} : {ex.Message}");
            return false;
        }
    }

    public static async Task<List<User>> GetUsersWithStateAsync(UserState state)
    {
        try
        {
            await using var db = new AppDbContext();
            return await db.Users
                .Where(u => u.State == state)
                .Include(u => u.Matches)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении списка пользователей: {ex.Message}");
            return new List<User>();
        }
    }

    public static async Task CreateMatchesAsync(List<Match> matches)
    {
        try
        {
            await using var db = new AppDbContext();
            await db.Matches.AddRangeAsync(matches);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи пар в базу данных: {ex.Message}");
        }
    }

    public static async Task UpdateUsersAsync(List<User> users)
    {
        try
        {
            await using var db = new AppDbContext();
            db.Users.UpdateRange(users);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка обновления пользователей: {ex.Message}");
        }
    }
}