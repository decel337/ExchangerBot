﻿using ExchangerBot.Bot.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Xml.Linq;
using ExchangerBot.Bot.Resources;

namespace ExchangerBot.Bot.States;

internal class RatesState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        if (Program.GlobalOrderService is null)
            return;

        List<List<string?>> rates = await Program.GlobalOrderService.GetAllRates();


        List<List<InlineKeyboardButton>> buttons =
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ];

        foreach (string name in Enum.GetNames(typeof(TakeCurrency)))
            if (name != "Unknown")
                buttons.Insert(0, [InlineKeyboardButton.WithCallbackData($"{SmileDictionary.CurrencyFlags[name]} {name}", $"rates_{name}")]);

        buttons.Insert(0, [InlineKeyboardButton.WithCallbackData($"💵 Все", $"rates_all")]);

        await bot.EditMessageText(chatId, messageId, "💱 Добро пожаловать в наш обменный пункт!\r\n\r\n🔍 Выберите валюту, которую хотите обменять,  \r\nили просмотрите 🔓 *все доступные курсы* 👇", replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}
