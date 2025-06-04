using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Globalization;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States;

internal class RatesSelectorState(TakeCurrency? currency = null) : IBotState
{
    Dictionary<string, int> _interpolateSumOfDefaultRate = new Dictionary<string, int>()
    {
        {"IDR", 10000000 },
        {"THB", 50000 },
        {"USD", 1000 },
        {"EUR", 1000 },
        {"AUD", 1000 },
        {"RUB", 100000 },
        {"UAH", 50000 },
        {"USDT", 1000 }
    };
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        if (Program.GlobalOrderService is null)
            return;

        List<List<string?>> rates = await Program.GlobalOrderService.GetAllRates();

        var buttons = new InlineKeyboardMarkup(
            [
                [InlineKeyboardButton.WithCallbackData("⬅️ Назад", "back")]
            ]);

        var customFormat = new NumberFormatInfo
        {
            NumberGroupSeparator = " ",
            NumberDecimalSeparator = ",",
            NumberDecimalDigits = 2 
        };

        string messageForUser = "Курс может меняться и зависеть от суммы. Всю информацию можно уточнить у менджера @ABCexchangebali\n\n";
        foreach (string take in Enum.GetNames(typeof(TakeCurrency)))
        {
            if (currency is not null && currency.ToString() != take)
                continue;

            if (take == "Unknown")
                continue;

            messageForUser += $"💱 Курсы обмена из {take} {SmileDictionary.CurrencyFlags[take]}:\n\n";

            foreach (string value in Enum.GetNames(typeof(Currency)))
            {
                if (value == "Unknown" || value == take)
                    continue;

                string? rate = rates.ToList().FirstOrDefault(x => x[0] == take && x[1] == value)?[2];
                if (rate != null)
                {
                    messageForUser += $"• {_interpolateSumOfDefaultRate[take].ToString("N0", customFormat)} {take} {SmileDictionary.CurrencyFlags[take]} = {Math.Round(double.Parse(rate, NumberStyles.Any, new CultureInfo("ru-RU")) * _interpolateSumOfDefaultRate[take] * 0.97, 2).ToString("N2", customFormat)} {value} {SmileDictionary.CurrencyFlags[value]}\n";
                }
                else
                {
                    messageForUser += $"• Нет данных для {take} {SmileDictionary.CurrencyFlags[take]} → {value} {SmileDictionary.CurrencyFlags[value]}\n";
                }
            }

            messageForUser += "\n\n";

        }

        await bot.EditMessageText(chatId, messageId, messageForUser, replyMarkup: buttons);
    }
}
