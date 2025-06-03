using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States;

internal class CooperationState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("⬅️ Главное меню", "back")]
        ]);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId),
            @"🤝 <b>По вопросам сотрудничества</b> пишите нашему менеджеру:
👉 <a href=""https://t.me/ABCexchangebali"">Связаться с менеджером</a>",
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
            replyMarkup: buttons);
    }
}