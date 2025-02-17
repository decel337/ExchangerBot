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

        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("🔄 Обмен", "exchange")],
            [InlineKeyboardButton.WithCallbackData("💳 Оплата счетов", "payments")],
            [InlineKeyboardButton.WithCallbackData("📈 Узнать курс", "rates")]
        ]);

        if (message.From is not null && message.From.IsBot)
        {
            await bot.EditMessageText(chatId, messageId, "👋 Привет! Выберите действие:", replyMarkup: buttons);
            return;
        }
        //await bot.DeleteMessage(chatId, messageId);
        //try
        //{
        //    await bot.DeleteMessage(chatId, messageId - 1);
        //}
        //catch{ }
        Message responseMessage = await bot.SendMessage(chatId, "👋 Привет! Выберите действие:", replyMarkup: buttons);
        stateManager.SetGeneralMessageId(responseMessage.Id);
    }
}
