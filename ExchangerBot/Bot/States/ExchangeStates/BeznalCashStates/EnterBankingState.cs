using ExchangerBot.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates.BeznalCashStates;

internal class EnterBankingState : IFormBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;
        await bot.DeleteMessage(chatId, messageId);

        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")],
                [InlineKeyboardButton.WithCallbackData("✅ Confirm", "back")]
            ];

        Order1 order = stateManager.GetOrder1(chatId);

        if(message.Text is not null)
            order.NameOfBank = message.Text;

        stateManager.SetOrder(chatId, order);

        await bot.EditMessageText(chatId, stateManager.GeneralMessageId, $"{order}", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}
