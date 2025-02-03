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
            [InlineKeyboardButton.WithCallbackData("🔄 Безналичные за наличные", "beznal_on_nal")],
            [InlineKeyboardButton.WithCallbackData("🔄 Наличный обмен", "only_nal")],
            [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
        ]);

        await bot.EditMessageText(chatId, messageId, "💱 Обмен валюты:\nВыберите действие:", replyMarkup: buttons);
    }
}
