using ExchangerBot.Bot.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangerBot.Bot.Database.Repositories;

internal class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByIdAsync(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetUsersByRoleAsync(Role role)
    {
        return await _context.Users.Where(u => u.Role == role).ToListAsync();
    }

    public async Task UpdateUserRoleAsync(long id, Role role)
    {
        User? user = await GetUserByIdAsync(id);
        if (user is not null)
        {
            user.Role = role;
            await _context.SaveChangesAsync();
        }
    }
}
