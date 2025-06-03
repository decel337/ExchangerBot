namespace ExchangerBot.Bot.Resources;

internal class SmileDictionary
{
    public static readonly Dictionary<string, string> CurrencyFlags = new()
    {
        { "IDR", "🇮🇩" }, // Индонезия
        { "THB", "🇹🇭" }, // Таиланд
        { "USD", "🇺🇸" }, // США
        { "EUR", "🇪🇺" }, // Евросоюз
        { "AUD", "🇦🇺" }, // Австралия
        { "RUB", "🇷🇺" }, // Россия
        { "UAH", "🇺🇦" }, // Украина
        { "USDT", "🪙" }  // Криптовалюта (можно также использовать 🤑, 💰)
    };
}
