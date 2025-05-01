using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.States.ExchangeStates.BeznalCashStates;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates.CashStates;

internal class PlatformEnterAmountState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        ]);

        IOrder order = stateManager.GetOrder(chatId);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}\n\n💰 Введите количество {order.TakeCurrency} для обмена:", replyMarkup: buttons);
        stateManager.SetState(chatId, new EnterAmountState());
    }
}
