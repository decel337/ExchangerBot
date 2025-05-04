using ExchangerBot.Bot.Database.Models;
using ExchangerBot.Bot.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot;

internal class UserService(IUserRepository userRepository, ITelegramBotClient botClient)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITelegramBotClient _botClient = botClient;

    public async Task AddUser(User user)
    {
        await _userRepository.AddUserAsync(user);
    }

    public async Task AssignManagerAsync(long userId)
    {
        await _userRepository.UpdateUserRoleAsync(userId, Role.Manager);
    }

    public async Task<List<User>> GetManagersAsync()
    {
        return await _userRepository.GetUsersByRoleAsync(Role.Manager);
    }

    public async Task NotifyManagersAsync(string message, long orderId)
    {
        List<User> managers = await GetManagersAsync();

        foreach (User manager in managers)
        {
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
            [
                [
                    InlineKeyboardButton.WithCallbackData("✅ Take", $"take_{orderId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData("❌ Cancel", $"cancel_{orderId}")
                ]
            ]);

            await _botClient.SendMessage(manager.Id, message, replyMarkup: inlineKeyboard);
        }

        //foreach (User manager in managers)
        //{
        //    InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
        //    [
        //        [
        //            InlineKeyboardButton.WithCallbackData("✏ Change sum", $"edit_{orderId}"),
        //        ],
        //        [
        //            InlineKeyboardButton.WithCallbackData("✅ Confirm", $"send_{orderId}")
        //        ],
        //        [
        //            InlineKeyboardButton.WithCallbackData("❌ Cancel", $"cancel_{orderId}")
        //        ]
        //    ]);

        //    await _botClient.SendMessage(manager.Id, message, replyMarkup: inlineKeyboard);
        //}

        //PAO maybe relocate to another method (for example notify user)
    }
}
