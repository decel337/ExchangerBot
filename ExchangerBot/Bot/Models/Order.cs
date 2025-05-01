using static System.Runtime.InteropServices.JavaScript.JSType;

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

public interface IOrder
{
    public int NumberOfOrder { get; init; }
    public int Amount { get; set; } 
    public TakeCurrency TakeCurrency { get; set; }
    public Currency Currency { get; set; }
    public PaymentMethod Method { get; set; }
    public string NameOfBank { get; set; }
    public string From { get; set; }
    public double SumOfPayment { get; set; }
    public double Rate { get; set; }
    public double RateToUsd { get; set; }
    public bool IsConfirmed { get; set; }
    public bool MayCalc { get; set; }
}

internal class OrderCrypto(int numberOfOrder) : IOrder
{
    public int NumberOfOrder { get; init; } = numberOfOrder;
    public int Amount { get; set; } = 0;
    public Currency Currency { get; set; } = Currency.Unknown;
    public TakeCurrency TakeCurrency { get; set; } = TakeCurrency.USDT;
    public PaymentMethod Method { get; set; } = PaymentMethod.Unknown;
    public string From { get; set; } = string.Empty;
    public string NameOfBank { get; set; } = string.Empty;
    public double SumOfPayment { get; set; } = 0;
    public bool IsConfirmed { get; set; } = false;
    public bool MayCalc { get; set; } = false;
    public double Rate { get; set; }
    public double RateToUsd { get; set; }

    public override string ToString()
    {
        if (MayCalc && SumOfPayment == 0)
            SumOfPayment = Tools.CalculateSumOfPayment(this);

        return $"Ваш заказ #{NumberOfOrder:D7}:\nКоличество USDT: {Amount}\nПолучаемая валюта: {Currency}\nСпособ получения валюты: {Method}\nКлиент: @{From}" +
            $"{(MayCalc ? $"\n\nПолучаете: {SumOfPayment:N0} {Currency}" : string.Empty)}";
    }
}

internal class OrderForBeznal(int numberOfOrder) : IOrder //for beznal
{
    public int NumberOfOrder { get; init; } = numberOfOrder;
    public int Amount { get; set; } = 0;
    public TakeCurrency TakeCurrency { get; set; } = TakeCurrency.Unknown;
    public Currency Currency { get; set; } = Currency.Unknown;
    public PaymentMethod Method { get; set; } = PaymentMethod.BankCard;
    public string NameOfBank { get; set; } = "Unknown";
    public string From { get; set; } = string.Empty;
    public double SumOfPayment { get; set; }
    public bool IsConfirmed { get; set; } = false;
    public bool MayCalc { get; set; } = false;
    public double Rate { get; set; }
    public double RateToUsd { get; set; }

    public override string ToString()
    {
        if (MayCalc && SumOfPayment == 0)
            SumOfPayment = Tools.CalculateSumOfPayment(this);

        return $"Ваш заказ #{NumberOfOrder:D7}:\nВалюта, которую отдаете: {TakeCurrency}\nКоличество: {Amount}\nПолучаемая валюта: {Currency}\nНазвание банка: {NameOfBank}\nКлиент: @{From}" +
            $"{(MayCalc ? $"\n\nПолучаете: {SumOfPayment:N0} {Currency}" : string.Empty)}";
    }
}

internal class OrderForNal(int numberOfOrder) : IOrder //for nal
{
    public int NumberOfOrder { get; init; } = numberOfOrder;
    public int Amount { get; set; } = 0;
    public TakeCurrency TakeCurrency { get; set; } = TakeCurrency.Unknown;
    public Currency Currency { get; set; } = Currency.Unknown;
    public PaymentMethod Method { get; set; } = PaymentMethod.Cash;
    public string NameOfBank { get; set; } = "Unknown";
    public string From { get; set; } = string.Empty;
    public double SumOfPayment { get; set; }
    public bool IsConfirmed { get; set; } = false;
    public bool MayCalc { get; set; } = false;
    public double Rate { get; set; }
    public double RateToUsd { get; set; }

    public override string ToString()
    {
        if (MayCalc && SumOfPayment == 0)
            SumOfPayment = Tools.CalculateSumOfPayment(this);

        return $"Ваш заказ #{NumberOfOrder:D7}:\nВалюта, которую отдаете: {TakeCurrency}\nКоличество: {Amount}\nПолучаемая валюта: {Currency}\nКлиент: @{From}" +
            $"{(MayCalc ? $"\n\nПолучаете: {SumOfPayment:N0} {Currency}" : string.Empty)}";
    }
}

internal static class Tools
{
    internal static double CalculateSumOfPayment(IOrder order)
    {
        if(Program.GlobalOrderService is null)
            return 0;

        (double rate, double rateToInterpolate, double rateToIdr) = Program.GlobalOrderService.GetRate(order.TakeCurrency, order.Currency).GetAwaiter().GetResult();

        order.Rate = rate;
        order.RateToUsd = rateToInterpolate;

        double sum = rate * order.Amount;

        double interpolatedSum = sum / rateToInterpolate;

        double commission = 0;

        if (interpolatedSum < 400)
        {
            commission = 200000 / rateToIdr;
        }

        if(interpolatedSum >= 400 && interpolatedSum < 1000)
        {
            commission = interpolatedSum * 0.03;
        }

        if (interpolatedSum >= 1000 && interpolatedSum < 5000)
        {
            commission = interpolatedSum * 0.025;
        }

        if (interpolatedSum >= 5000 && interpolatedSum < 10000)
        {
            commission = interpolatedSum * 0.02;
        }

        if (interpolatedSum >= 10000)
        {
            commission = interpolatedSum * 0.015;
        }

        interpolatedSum -= commission;

        return Math.Round(interpolatedSum * rateToInterpolate);
    }
}
