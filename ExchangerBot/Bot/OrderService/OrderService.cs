using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.States;
using System.Globalization;


namespace ExchangerBot.Bot.OrderService;

public class OrderService
{
    private const string SHEET_ID = "1cGbExvem2DX-LQyTnBFYl8UVfkFYuSlSeiKf8oXzeko";
    private readonly SheetsService _sheetsService;
    public OrderService()
    {
        GoogleCredential credential = GoogleCredential.FromFile("excelaccess.json")
            .CreateScoped(SheetsService.Scope.Spreadsheets);

        _sheetsService = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential
        });
    }

    private void EnsureSheetExists()
    {
        string sheetName = DateTime.Now.ToString("dd.MM");

        var spreadsheet = _sheetsService.Spreadsheets.Get(SHEET_ID).Execute();
        bool sheetExists = spreadsheet.Sheets.Any(s => s.Properties.Title == sheetName);

        if (!sheetExists)
        {
            var addSheetRequest = new AddSheetRequest
            {
                Properties = new SheetProperties { Title = sheetName }
            };

            var batchUpdateRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request> { new Request { AddSheet = addSheetRequest } }
            };

            _sheetsService.Spreadsheets.BatchUpdate(batchUpdateRequest, SHEET_ID).Execute();

            var headers = new List<object>
                    {
                        "Номер заказа",
                        "Метод",
                        "Получаемая сума",
                        "Валюта клиента",
                        "Отдаем в валюте",
                        "Какой курс на данный момент",
                        "Клиент",
                        "Банк",
                        "Всего к оплате клиенту",
                        "в $",
                        "Сумма без комиссий",
                        "в $",
                        "Имя курьера",
                        "Стоимость курьера",
                        "Реферал",
                        "Доход $"
                    };

            var headerRange = $"{sheetName}!A1";
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>> { headers }
            };

            var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, SHEET_ID, headerRange);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            updateRequest.Execute();

            //Second part
            var formulaHeadersRange = $"{sheetName}!R14:T14";
            var formulaHeaders = new List<IList<object>>
            {
                new List<object> { "Количество заказов", "Общий доход за день", "Общий доход за месяц" }
            };

            var headersRequest = _sheetsService.Spreadsheets.Values.Update(
                new ValueRange { Values = formulaHeaders },
                SHEET_ID,
                formulaHeadersRange
            );
            headersRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            headersRequest.Execute();

            var formulaRange = $"{sheetName}!R15:T15";

            DateTime today = DateTime.Now;

            DateTime? closestPreviousDate = null;
            string? closestPreviousSheet = null;

            foreach (var sheet in spreadsheet.Sheets)
            {
                string title = sheet.Properties.Title;

                if (DateTime.TryParseExact(title, "dd.MM", null, DateTimeStyles.None, out DateTime sheetDate))
                {
                    if (sheetDate.Month == today.Month && sheetDate.Year == today.Year && sheetDate < today)
                    {
                        if (closestPreviousDate == null || sheetDate > closestPreviousDate)
                        {
                            closestPreviousDate = sheetDate;
                            closestPreviousSheet = title;
                        }
                    }
                }
            }

            string monthlyIncomeFormula = $"=SUM(P2:P)";

            if (closestPreviousSheet != null)
                monthlyIncomeFormula = $"='{closestPreviousSheet}'!T15 + SUM(P2:P)";



            var formulas = new List<IList<object>>
            {
                new List<object>
                {
                    "=COUNTA(A2:A)",
                    $"=SUM(P2:P)",
                    monthlyIncomeFormula
                }
            };

            var formulaRequest = _sheetsService.Spreadsheets.Values.Update(
                new ValueRange { Values = formulas },
                SHEET_ID,
                formulaRange
            );
            formulaRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            formulaRequest.Execute();
        }
    }

    public void WriteOrder(IOrder order)
    {
        EnsureSheetExists();
        string sheetName = DateTime.Now.ToString("dd.MM");
        string range = $"{sheetName}!A:A"; // Проверяем колонку A для поиска первой пустой строки
        var request = _sheetsService.Spreadsheets.Values.Get(SHEET_ID, range);
        var response = request.Execute();
        int nextRow = (response.Values != null) ? response.Values.Count + 1 : 1;

        string writeRange = $"{sheetName}!A{nextRow}";

        var orderValues = new List<object>
            {
                $"{order.NumberOfOrder:D7}",
                order.Method.ToString(),
                order.Amount,
                order.TakeCurrency.ToString(),
                order.Currency.ToString(),
                order.Rate,
                order.From,
                order.NameOfBank,
                order.SumOfPayment,
                order.SumOfPayment / order.RateToUsd,
                Math.Round(order.Rate * order.Amount, 2), //сумма без комиссий
                Math.Round(order.Rate * order.Amount / order.RateToUsd, 2), //сумма без комиссий usd
                string.Empty,
                string.Empty,
                string.Empty,
                $"=L{nextRow}-J{nextRow}-N{nextRow}"
            };

        var valueRange = new ValueRange { Values = new List<IList<object>> { { orderValues } } };
        var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, SHEET_ID, writeRange);
        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

        updateRequest.Execute();
        Console.WriteLine($"Order written in {sheetName}, row {nextRow}: {string.Join(" | ", orderValues)}");
    }

    public async Task<(double, double, double)> GetRate(TakeCurrency takeCurrency, Currency giveCurrency)
    {
        string range = "Rates!A1:C";
        ValueRange response = await _sheetsService.Spreadsheets.Values.Get(SHEET_ID, range).ExecuteAsync();
        IList<object>? rates = response.Values.Where(row => row[0].ToString() == takeCurrency.ToString() &&
            row[1].ToString() == giveCurrency.ToString()).FirstOrDefault();

        string? rate = rates?[2].ToString(); // rate for original

        rates = response.Values.Where(row => row[0].ToString() == "USD" &&
            row[1].ToString() == giveCurrency.ToString()).FirstOrDefault();

        string? rate1 = rates?[2].ToString(); // rate for convert to usd

        rates = response.Values.Where(row => row[0].ToString() == "USD" &&
            row[1].ToString() == "IDR").FirstOrDefault();

        string? rate2 = rates?[2].ToString(); // rate for convert to idr

        rate ??= 0.ToString();
        rate1 ??= 0.ToString();
        rate2 ??= 0.ToString();

        return (double.Parse(rate, NumberStyles.Any, new CultureInfo("ru-RU")),
            double.Parse(rate1, NumberStyles.Any, new CultureInfo("ru-RU")),
            double.Parse(rate2, NumberStyles.Any, new CultureInfo("ru-RU")));
    }

    public async Task<List<List<string?>>> GetAllRates()
    {
        string range = "Rates!A1:C";
        ValueRange response = await _sheetsService.Spreadsheets.Values.Get(SHEET_ID, range).ExecuteAsync();
        List<List<string?>> rates = response.Values.Select(x => x.Select(value => value.ToString()).ToList()).ToList();

        return rates;
    }
}
