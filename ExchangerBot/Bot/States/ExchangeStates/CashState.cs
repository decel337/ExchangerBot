using ExchangerBot.Bot.Models;
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
            if (name == "Unknown" || name == "UAH" || name == "RUB" || name == "USDT") //add excluded currency
                continue;

            buttons.Add([InlineKeyboardButton.WithCallbackData($"💵 {name}", $"select_take_currency1:{name}")]);
        }

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}\n\n💰 Выберите валюту, которую отдаете", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}
