using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ExchangerBot.Bot.States;

internal class RatesState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("💲 USD", "rate_usd")],
            [InlineKeyboardButton.WithCallbackData("💶 EUR", "rate_eur")],
            [InlineKeyboardButton.WithCallbackData("₿ BTC", "rate_btc")],
            [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        ]);

        await bot.EditMessageText(chatId, messageId, "📊 Курсы валют:\nВыберите валюту:", replyMarkup: buttons);
    }
}
