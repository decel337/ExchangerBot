using ExchangerBot.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates;

internal class BeznalCashState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        Order1 order = new();
        stateManager.SetOrder(chatId, order);

        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ];

        foreach (string name in Enum.GetNames(typeof(TakeCurrency)))
            if (name != "Unknown")
                buttons.Add([InlineKeyboardButton.WithCallbackData(name, $"select_take_currency:{name}")]);

        await bot.EditMessageText(chatId, stateManager.GeneralMessageId, $"{order}\nВыберите валюту, которую отдаете", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}
