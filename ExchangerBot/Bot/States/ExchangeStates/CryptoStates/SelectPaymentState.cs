using ExchangerBot.Bot.Models;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ExchangerBot.Bot.States.ExchangeStates.CryptoStates;

internal class SelectPaymentState : IFormBotState
{
    private readonly Dictionary<string, string> _normalizeNames = new Dictionary<string, string>()
    {
        {"Cash", "Доставка курьером" },
        {"BankCard", "Перевод на карту" },
    };

    private readonly Dictionary<string, string> _normalizeSmile = new Dictionary<string, string>()
    {
        {"Cash", "💸" },
        {"BankCard", "💳" },
    };

    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;
        IOrder order = stateManager.GetOrder(chatId);

        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ];

        foreach (string name in Enum.GetNames(typeof(PaymentMethod)))
        {
            if (name == "Unknown" || name == "ATM")
                continue;
            buttons.Insert(0, [InlineKeyboardButton.WithCallbackData($"{_normalizeSmile[name]} {_normalizeNames[name]}", $"select_payment:{name}")]);
        }

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}\n\n💳 <b>Выберите способ получения денег: </b>", 
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, 
            replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}