using ExchangerBot.Bot.Database.Models;

namespace ExchangerBot.Bot.Database;

public static class DatabaseInitializer
{
    public static void Initialize()
    {
        using var context = new AppDbContext();
        context.Database.EnsureCreated();

        if(!context.Users.Any(u=>u.Role == Role.Manager))
        {
            context.Users.Add(new User { Id = 520083750, Name = "Manager", Role = Role.Manager });
            context.Users.Add(new User { Id = 5766932417, Name = "Manager", Role = Role.Manager });
            context.SaveChanges();
        }

        //if (!context.Users.Any(u => u.Role == Role.Admin))
        //{
        //    context.Users.Add(new User { Id = 123456789, Name = "Admin", Role = Role.Admin });
        //    context.SaveChanges();
        //}
    }
}
