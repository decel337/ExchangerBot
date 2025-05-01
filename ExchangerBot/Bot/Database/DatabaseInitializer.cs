using ExchangerBot.Bot.Database.Models;

namespace ExchangerBot.Bot.Database;

public static class DatabaseInitializer
{
    public static void Initialize()
    {
        using AppDbContext context = new();
        context.Database.EnsureCreated();

        context.Users.RemoveRange(context.Users);
        context.SaveChanges();

        if (!context.Users.Any(u=>u.Role == Role.Manager))
        {
            context.Users.Add(new User { Id = 5766932417, Name = "ManagerABC", Role = Role.Manager });
            context.Users.Add(new User { Id = 6588331612, Name = "TestManager", Role = Role.Manager });
            context.SaveChanges();
        }

        //if (!context.Users.Any(u => u.Role == Role.Admin))
        //{
        //    context.Users.Add(new User { Id = 123456789, Name = "Admin", Role = Role.Admin });
        //    context.SaveChanges();
        //}
    }
}
