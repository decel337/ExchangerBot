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
                         [InlineKeyboardButton.WithCallbackData("⬅️ Главное меню", "back")]
                    ]);

                await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"Дождитесь сообщения от менеджера @ABCexchangebali \n\n{order}", replyMarkup: buttons1);
                return;
            }
        }
        catch (ArgumentNullException)
        {

        }


        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("🪙 Криптовалюта (USDT)", "crypto")],
            [InlineKeyboardButton.WithCallbackData("💳 Безналичные", "beznalcash")],
            [InlineKeyboardButton.WithCallbackData("🤝 Наличные", "cash")],
            [InlineKeyboardButton.WithCallbackData("🏧 Выдача наличных через банкомат", "atm")],
            [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        ]);



        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), "💼 Способы обмена валют:\r\nВыберите подходящий формат — мы адаптируемся под ваши предпочтения \U0001f91d", replyMarkup: buttons);
    }
}
