using ExchangerBot.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates.CashStates;

internal class ConfirmationState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")],
                [InlineKeyboardButton.WithCallbackData("✅ Confirm", "back")]
            ];

        Order2 order = stateManager.GetOrder2(chatId);

        await bot.EditMessageText(chatId, stateManager.GeneralMessageId, $"{order}", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}
