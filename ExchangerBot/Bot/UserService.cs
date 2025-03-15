using ExchangerBot.Bot.Database.Models;
using ExchangerBot.Bot.Database.Repositories;
using Telegram.Bot;

namespace ExchangerBot.Bot;

internal class UserService(IUserRepository userRepository, ITelegramBotClient botClient)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITelegramBotClient _botClient = botClient;

    public async Task AssignManagerAsync(long userId)
    {
        await _userRepository.UpdateUserRoleAsync(userId, Role.Manager);
    }

    public async Task<List<User>> GetManagersAsync()
    {
        return await _userRepository.GetUsersByRoleAsync(Role.Manager);
    }

    public async Task NotifyManagersAsync(string message)
    {
        List<User> managers = await GetManagersAsync();
        foreach (User manager in managers)
        {
            await _botClient.SendMessage(manager.Id, message);
        }
    }
}
