using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.States.ExchangeStates.CryptoStates;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates.BeznalCashStates;

internal class PlatformEnterAmountState : IFormBotState
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

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}\n\n💰 <b>Введите количество {order.TakeCurrency} для обмена ⬇️: </b>", replyMarkup: buttons, parseMode: ParseMode.Html);
        stateManager.SetState(chatId, new EnterAmountState());
    }
}
