using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates.CryptoStates;
internal class EnterAmountUsdtState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        var buttons = new InlineKeyboardMarkup(
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ]);

        if (!decimal.TryParse(message.Text, out decimal amount) || amount <= 0)
        {
            await bot.EditMessageText(chatId, messageId, "Ошибка: введите корректное число.", replyMarkup: buttons);
            return;
        }

        stateManager.SetUserData(message.Chat.Id, "USDTAmount", amount);

        var buttons = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("USD", "currency_usd") },
            new[] { InlineKeyboardButton.WithCallbackData("EUR", "currency_eur") },
            new[] { InlineKeyboardButton.WithCallbackData("RUB", "currency_rub") }
        });

        await bot.SendTextMessageAsync(message.Chat.Id, "Выберите валюту для получения наличных:", replyMarkup: buttons);

        stateManager.SetState(message.Chat.Id, new SelectCurrencyState());
    }
}
