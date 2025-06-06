﻿using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.Resources;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ExchangeStates.CryptoStates;
internal class EnterAmountUsdtState : IFormBotState
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

        foreach (string name in Enum.GetNames(typeof(Currency)))
        {
            if (name == "Unknown")
                continue;
            buttons.Insert(0, [InlineKeyboardButton.WithCallbackData($"{SmileDictionary.CurrencyFlags[name]} {name}", $"select_currency:{name}")]);
        }

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId), $"{order}\n\n💰 <b>Выберите валюту для получения наличных:</b>",
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

        //stateManager.SetState(chatId, new SelectCurrencyState());
    }
}
