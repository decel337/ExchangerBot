﻿using ExchangerBot.Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ManagerStates;

internal class ConfirmedOrderState : IFormBotState
{
    private readonly long _orderId;
    private readonly UserService _userService;
    public ConfirmedOrderState(long orderId, UserService userService)
    {
        _orderId = orderId;
        _userService = userService;
    }
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        await bot.SendMessage(_orderId, "Менеджер принялся за ваш заказ! Ожидайте сообщений для уточнения!"); //PAO выделить все оповещения юзера в отдельный метод!!

        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
        [
            [
                            InlineKeyboardButton.WithCallbackData("✏ Change sum", $"edit_{_orderId}"),
                        ],
                        [
                            InlineKeyboardButton.WithCallbackData("✅ Confirm", $"send_{_orderId}")
                        ],
                        [
                            InlineKeyboardButton.WithCallbackData("❌ Cancel", $"cancel_{_orderId}")
                        ]
        ]);
        await bot.EditMessageText(message.Chat.Id, message.Id, message.Text ?? "ERROR", replyMarkup: inlineKeyboard);

    }
}
