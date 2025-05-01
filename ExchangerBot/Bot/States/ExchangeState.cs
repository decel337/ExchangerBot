using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ExchangerBot.Bot.States;

internal class ExchangeState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        try
        {
            Models.IOrder order = stateManager.GetOrder(chatId);
            if (order.IsConfirmed)
            {
                var buttons1 = new InlineKeyboardMarkup( //duplicate from main menu state
                    [
                         [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
                    ]);

                await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"Обмен недоступен, ваш ордер в обработке. Дождитесь сообщения от менеджера! \n\n{order}", replyMarkup: buttons1);
                return;
            }
        }
        catch (ArgumentNullException)
        {

        }


        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("🔄 Криптовалюта", "crypto")],
            [InlineKeyboardButton.WithCallbackData("🔄 Наличные за безналичные", "beznalcash")],
            [InlineKeyboardButton.WithCallbackData("🔄 Наличный обмен", "cash")],
            [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        ]);



        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), "💱 Варианты обмена валют:", replyMarkup: buttons);
    }
}
