using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.States.ExchangeStates.CryptoStates;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates;

internal class CryptoState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        ]);

        OrderCrypto order = new(stateManager.CountOfOrder);
        order.From = message.Chat.Username ?? "guest";
        stateManager.SetOrder(chatId, order);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}\n\n💰 Введите количество USDT для обмена:", replyMarkup: buttons);
        stateManager.SetState(chatId, new EnterAmountUsdtState());
    }
}
