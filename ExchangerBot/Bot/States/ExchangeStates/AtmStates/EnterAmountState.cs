﻿using ExchangerBot.Bot.Models;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace ExchangerBot.Bot.States.ExchangeStates.AtmStates;

internal class EnterAmountState : IFormBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;
        await bot.DeleteMessage(chatId, messageId);

        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ];

        IOrder order = stateManager.GetOrder(chatId);

        if (!int.TryParse(message.Text, out int amount) || amount <= 0)
        {
            await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}\n\n🛑 Ошибка: введите корректное число.", replyMarkup: new InlineKeyboardMarkup(buttons));
            return;
        }

        order.Amount = amount;

        stateManager.SetOrder(chatId, order);


        buttons =
            [
                [InlineKeyboardButton.WithCallbackData("✅ Confirm", "confirm")],
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ];

        order = stateManager.GetOrder(chatId);
        order.MayCalc = true;
        stateManager.SetOrder(chatId, order);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}
