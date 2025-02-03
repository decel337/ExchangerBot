namespace ExchangerBot.Bot.Models;

internal class Order
{
    public static int UsdtAmount = 0;
    public static Currency Currency = Currency.Unknown;
    public static PaymentMethod Method = PaymentMethod.Unknown;

    public override string ToString()
    {
        return $"Ваш заказ:\nКоличество USDT: {UsdtAmount}\nПолучаемая валюта: {Currency}\nНал или безнал: {Method}";
    }
}
