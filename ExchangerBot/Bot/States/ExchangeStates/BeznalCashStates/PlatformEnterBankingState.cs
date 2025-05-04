using ExchangerBot.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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

        IOrder order = stateManager.GetOrder(chatId);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}\n\n🏧 <b>Введите банк, на котором деньги ⬇️:</b>", replyMarkup: buttons, parseMode: ParseMode.Html);
        stateManager.SetState(chatId, new EnterBankingState());
    }
}
