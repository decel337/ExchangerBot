using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ExchangerBot.Bot.States;

internal class MainMenuState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        string messageExchange = "🔄 Обмен";

        try
        {

            Models.IOrder order = stateManager.GetOrder(chatId);
            if (order.IsConfirmed)
                messageExchange = "🧾 Ваш заказ";
        }
        catch (ArgumentNullException)
        {

        }


        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData(messageExchange, "exchange")],
            [InlineKeyboardButton.WithCallbackData("💳 Оплата счетов", "payments")],
            [InlineKeyboardButton.WithCallbackData("📈 Узнать курс", "rates")]
        ]);

        if (message.From is not null && message.From.IsBot)
        {
            await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"👋 Привет, {message?.Chat?.FirstName}! Выберите действие:", replyMarkup: buttons);
            return;
        }
        //await bot.DeleteMessage(chatId, messageId);
        //try
        //{
        //    await bot.DeleteMessage(chatId, messageId - 1);
        //}
        //catch{ }
        Message responseMessage = await bot.SendMessage(chatId, $"👋 Привет, {message?.From?.FirstName}! Выберите действие:", replyMarkup: buttons);
        stateManager.SetGeneralMessageId(chatId, responseMessage.Id);
    }
}
