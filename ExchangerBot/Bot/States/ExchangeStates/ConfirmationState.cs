using ExchangerBot.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates;

internal class ConfirmationState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("✅ Confirm", "confirm")],
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ];

        Order order = stateManager.GetOrder(chatId);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}", replyMarkup: new InlineKeyboardMarkup(buttons));

    }
}
