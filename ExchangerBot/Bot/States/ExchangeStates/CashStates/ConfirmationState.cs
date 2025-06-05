using ExchangerBot.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates.CashStates;

internal class ConfirmationState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("✅ Подтвердить заказ", "confirm2")],
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ];

        IOrder order = stateManager.GetOrder(chatId);
        order.MayCalc = true;
        stateManager.SetOrder(chatId, order);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}
