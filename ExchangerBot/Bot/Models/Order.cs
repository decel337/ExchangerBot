namespace ExchangerBot.Bot.Models;

internal class Order
{
    public int UsdtAmount = 0;
    public Currency Currency = Currency.Unknown;
    public PaymentMethod Method = PaymentMethod.Unknown;

    public override string ToString()
    {
        return $"Ваш заказ:\nКоличество USDT: {UsdtAmount}\nПолучаемая валюта: {Currency}\nНал или безнал: {Method}";
    }
}
