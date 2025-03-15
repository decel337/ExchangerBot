using ExchangerBot.Bot.Database.Models;

namespace ExchangerBot.Bot.Database.Repositories;

internal interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(long id);
    Task<List<User>> GetUsersByRoleAsync(Role role);
    Task UpdateUserRoleAsync(long id, Role role);
}
