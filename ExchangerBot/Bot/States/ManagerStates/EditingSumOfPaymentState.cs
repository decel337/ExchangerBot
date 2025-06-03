using ExchangerBot.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExchangerBot.Bot.States.ManagerStates;

internal class EditingSumOfPaymentState : IFormBotState
{
    private readonly long _orderId;
    private readonly UserService _userService;
    public EditingSumOfPaymentState(long orderId, UserService userService)
    {
        _orderId = orderId;
        _userService = userService;
    }
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        if (message.From is not null && message.From.IsBot)
            return;

        long chatId = message.Chat.Id;
        int messageId = message.MessageId;
        IOrder order = stateManager.GetOrder(_orderId);

        if (!int.TryParse(message.Text, out int amount) || amount <= 0)
        {
            await bot.SendMessage(chatId, "🛑 Ошибка: введите корректное число.");
            return;
        }

        order.SumOfPayment = amount;
        stateManager.SetOrder(_orderId, order);
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
[
    [
                            InlineKeyboardButton.WithCallbackData("✏ Change sum", $"edit_{_orderId}"),
                        ],
                        [
                            InlineKeyboardButton.WithCallbackData("✅ Confirm", $"send_{_orderId}")
                        ],
                        [
                            InlineKeyboardButton.WithCallbackData("❌ Cancel", $"cancel_{_orderId}")
                        ]
]);
        await bot.SendMessage(chatId, order.ToString()!, replyMarkup: inlineKeyboard);
        //await _userService.NotifyManagersAsync(stateManager.GetOrder(_orderId).ToString()!, _orderId);
        //        await bot.SendTextMessageAsync(
        //    chatId: _orderId,
        //    text: "‼️ Сумма вашего ордера была изменена!",
        //    replyToMessageId: orderMessage.MessageId,
        //    replyMarkup: keyboard
        //);

        ReplyParameters replyParameters = new()
        {
            MessageId = stateManager.GetGeneralMessageId(_orderId),
            AllowSendingWithoutReply = true
        };
        await bot.SendMessage(_orderId, "<b>Сумма вашего ордера была изменена, обратите внимание!</b>\nАктуальную сумму смотрите в меню — <b>Ваш заказ</b>",
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyParameters: replyParameters); //PAO user notify

    }
}
