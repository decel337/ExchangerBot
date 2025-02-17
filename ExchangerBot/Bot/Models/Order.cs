namespace ExchangerBot.Bot.Models;

//internal inter Order
//{
//    public int UsdtAmount = 0;
//    public Currency Currency = Currency.Unknown;
//    public PaymentMethod Method = PaymentMethod.Unknown;

//    public override string ToString()
//    {
//        return $"Ваш заказ:\nКоличество USDT: {UsdtAmount}\nПолучаемая валюта: {Currency}\nНал или безнал: {Method}";
//    }
//}
internal class Order //for crypto
{
    public int UsdtAmount = 0;
    public Currency Currency = Currency.Unknown;
    public PaymentMethod Method = PaymentMethod.Unknown;

    public override string ToString()
    {
        return $"Ваш заказ:\nКоличество USDT: {UsdtAmount}\nПолучаемая валюта: {Currency}\nНал или безнал: {Method}";
    }
}

internal class Order1 //for crypto
{
    public int Amount = 0;
    public TakeCurrency TakeCurrency = TakeCurrency.Unknown;
    public Currency Currency = Currency.Unknown;
    public string NameOfBank = "Unknown";

    public override string ToString()
    {
        return $"Ваш заказ:\nВалюта, которую отдаете: {TakeCurrency}\nКоличество: {Amount}\nПолучаемая валюта: {Currency}\nНазвание банка: {NameOfBank}";
    }
}
