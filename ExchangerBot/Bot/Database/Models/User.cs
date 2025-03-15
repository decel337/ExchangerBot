namespace ExchangerBot.Bot.Database.Models;

internal class User
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public Role Role { get; set; } = Role.User;
}
