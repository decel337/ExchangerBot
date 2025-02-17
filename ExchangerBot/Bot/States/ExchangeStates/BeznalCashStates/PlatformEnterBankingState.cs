using ExchangerBot.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates.BeznalCashStates;

internal class PlatformEnterBankingState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        ]);

        Order1 order = stateManager.GetOrder1(chatId);

        await bot.EditMessageText(chatId, messageId, $"{order}\nВведите банк, на котором деньги:", replyMarkup: buttons);
        stateManager.SetState(chatId, new EnterBankingState());
    }
}
