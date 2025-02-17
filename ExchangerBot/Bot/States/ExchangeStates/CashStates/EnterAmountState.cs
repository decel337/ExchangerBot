using ExchangerBot.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates.CashStates;

internal class EnterAmountState : IFormBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;
        await bot.DeleteMessage(chatId, messageId);

        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ];

        Order2 order = stateManager.GetOrder2(chatId);

        if (!int.TryParse(message.Text, out int amount) || amount <= 0)
        {
            await bot.EditMessageText(chatId, stateManager.GeneralMessageId, $"{order}\nОшибка: введите корректное число.", replyMarkup: new InlineKeyboardMarkup(buttons));
            return;
        }

        order.Amount = amount;

        stateManager.SetOrder(chatId, order);

        foreach (string name in Enum.GetNames(typeof(Currency)))
            if (name != "Unknown")
                buttons.Add([InlineKeyboardButton.WithCallbackData(name, $"select_currency2:{name}")]);

        await bot.EditMessageText(chatId, stateManager.GeneralMessageId, $"{order}\nВыберите валюту для получения наличных:", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}
