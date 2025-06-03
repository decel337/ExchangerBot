using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates;

internal class CashState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        OrderForNal order = new(stateManager.CountOfOrder);
        order.From = message.Chat.Username ?? "guest";
        stateManager.SetOrder(chatId, order);

        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ];

        foreach (string name in Enum.GetNames(typeof(TakeCurrency)))
        {
            if (name == "Unknown" || name == "UAH" || name == "USDT" || name == "THB") //add excluded currency
                continue;

            buttons.Insert(0, [InlineKeyboardButton.WithCallbackData($"{SmileDictionary.CurrencyFlags[name]} {name}", $"select_take_currency1:{name}")]);
        }

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}\n\n💰 <b>Выберите валюту, которую отдаете </b>",
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}
