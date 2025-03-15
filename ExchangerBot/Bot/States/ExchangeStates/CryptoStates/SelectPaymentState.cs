using ExchangerBot.Bot.Models;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ExchangerBot.Bot.States.ExchangeStates.CryptoStates;

internal class SelectPaymentState : IFormBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;
        Order order = stateManager.GetOrder(chatId);

        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ];

        foreach (string name in Enum.GetNames(typeof(PaymentMethod)))
            if (name != "Unknown")
                buttons.Add([InlineKeyboardButton.WithCallbackData($"💳 {name}", $"select_payment:{name}")]);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}\n\n💳 Выберите способ получения денег:", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}