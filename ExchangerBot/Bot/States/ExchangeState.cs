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

        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("🔄 Криптовалюта", "crypto")],
            [InlineKeyboardButton.WithCallbackData("🔄 Безналичные за наличные", "beznalcash")],
            [InlineKeyboardButton.WithCallbackData("🔄 Наличный обмен", "cash")],
            [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        ]);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), "💱 Варианты обмена валют:", replyMarkup: buttons);
    }
}
