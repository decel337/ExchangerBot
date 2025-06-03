using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ExchangerBot.Bot.States;

internal class PaymentsState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        ]);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), "💳 <b>Оплата счетов</b>\r\n\r\n⚠️ Этот раздел временно в разработке — но вы не останетесь без помощи!\r\n\r\n📞 Свяжитесь с менеджером прямо сейчас:\r\n👉 <a href=\"https://t.me/ABCexchangebali\">@ABCexchangebali</a> — он подскажет и поможет 💬",
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: buttons);

        //long chatId = message.Chat.Id;
        //int messageId = message.MessageId;

        //var buttons = new InlineKeyboardMarkup(
        //[
        //    [InlineKeyboardButton.WithCallbackData("🏦 Банковский перевод", "payment_bank")],
        //    [InlineKeyboardButton.WithCallbackData("💳 Кредитная карта", "payment_card")],
        //    [InlineKeyboardButton.WithCallbackData("📲 Мобильный платеж", "payment_mobile")],
        //    [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        //]);

        //await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), "💰 Оплата счетов:\nВыберите метод оплаты:", replyMarkup: buttons);
    }
}
