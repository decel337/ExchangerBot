using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExchangerBot.Bot.States.ManagerStates;

internal class CanceledOrderState : IFormBotState
{
    private readonly long _orderId;
    private readonly UserService _userService;
    public CanceledOrderState(long orderId, UserService userService)
    {
        _orderId = orderId;
        _userService = userService;
    }
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        await bot.SendMessage(_orderId, "Ваш заказ отклонен! Ожидайте сообщение на счет отказа.\n/start"); //PAO выделить все оповещения юзера в отдельный метод!!

        await bot.DeleteMessage(message.Chat.Id, message.Id);

    }
}
