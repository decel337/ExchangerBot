using ExchangerBot.Bot.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangerBot.Bot.Database;

internal class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=bot.db");
    }
}
