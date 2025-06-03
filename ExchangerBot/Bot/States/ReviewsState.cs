using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States;

internal class ReviewsState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        ]);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId),
            @"🗣 <b>Отзывы наших клиентов</b> доступны в нашей группе:
👉 <a href=""https://t.me/abcxchange/4"">Читать отзывы</a>",
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
            replyMarkup: buttons);
    }
}