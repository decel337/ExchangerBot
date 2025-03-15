using ExchangerBot.Bot.States;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using ExchangerBot.Bot.States.ExchangeStates;
using ExchangerBot.Bot.States.ExchangeStates.CryptoStates;
using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.States.ExchangeStates.BeznalCashStates;

namespace ExchangerBot.Bot;

internal class BotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly UserService _userService;
    private readonly StateManager _stateManager;
    private readonly MessageManager _messageManager;

    public BotService(ITelegramBotClient botClient, UserService userService)
    {
        _botClient = botClient;
        _userService = userService;
        _stateManager = new StateManager();
        _messageManager = new MessageManager();
    }

    public async Task HandleUpdate(Update update)
    {
        if (update.Type == UpdateType.Message && update.Message?.Text == "/start")
            await ShowMainMenu(update.Message);
        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery is not null)
            await HandleCallbackQuery(update.CallbackQuery);
        else if(update.Message is not null && _stateManager.GetState(update.Message.Chat.Id) is IFormBotState)
            await HandleTextMessage(update.Message);
    }

    private async Task ShowMainMenu(Message? message)
    {
        if (message == null)
            return;

        MainMenuState mainMenu = new();
        await mainMenu.Handle(_botClient, message, _stateManager);
        _stateManager.SetState(message.Chat.Id, mainMenu);
        _messageManager.AddSentMessage(message.Chat.Id, message.Id);
    }

    private async Task HandleTextMessage(Message message)
    {
        //await _messageManager.DeleteSentMessages(_botClient, query.Message.Chat.Id);

        IBotState currentState = _stateManager.GetState(message.Chat.Id);

        await currentState.Handle(_botClient, message, _stateManager);


        //_messageManager.AddSentMessage(query.Message.Chat.Id, query.Message.Id);
    }

    private async Task HandleCallbackQuery(CallbackQuery query)
    {
        if (query.Message == null)
        {
            await _botClient.AnswerCallbackQuery(query.Id, "Error: message not found.");
            return;
        }

        //await _messageManager.DeleteSentMessages(_botClient, query.Message.Chat.Id);

        switch (query.Data)
        {
            //Main menu
            case "exchange":
                _stateManager.SetState(query.Message.Chat.Id, new ExchangeState());
                break;
            case "payments":
                _stateManager.SetState(query.Message.Chat.Id, new PaymentsState());
                break;
            case "rates":
                _stateManager.SetState(query.Message.Chat.Id, new RatesState());
                break;
            case "back":
                _stateManager.SetState(query.Message.Chat.Id, new MainMenuState());
                break;
            //Exhange crypto
            case "crypto":
                _stateManager.SetState(query.Message.Chat.Id, new CryptoState());
                break;
            //Selected currency CRYPTO
            case string currency when currency.StartsWith("select_currency:"):
                Models.Order order = _stateManager.GetOrder(query.Message.Chat.Id);
                _ = Enum.TryParse(currency.Split(':')[1], out Currency currentCurrency);
                order.Currency = currentCurrency;
                _stateManager.SetOrder(query.Message.Chat.Id, order);
                _stateManager.SetState(query.Message.Chat.Id, new SelectPaymentState());
                break;
            //Selected currency CRYPTO
            case string payment when payment.StartsWith("select_payment:"):
                order = _stateManager.GetOrder(query.Message.Chat.Id);
                _ = Enum.TryParse(payment.Split(':')[1], out PaymentMethod currentPayment);
                order.Method = currentPayment;
                _stateManager.SetOrder(query.Message.Chat.Id, order);
                _stateManager.SetState(query.Message.Chat.Id, new ConfirmationState());
                break;
            //Exhange Beznal on cash
            case "beznalcash":
                _stateManager.SetState(query.Message.Chat.Id, new BeznalCashState());
                break;
            case string currency when currency.StartsWith("select_take_currency:"):
                Order1 order1 = _stateManager.GetOrder1(query.Message.Chat.Id);
                _ = Enum.TryParse(currency.Split(':')[1], out TakeCurrency currentTakeCurrency);
                order1.TakeCurrency = currentTakeCurrency;
                _stateManager.SetOrder(query.Message.Chat.Id, order1);
                _stateManager.SetState(query.Message.Chat.Id, new PlatformEnterAmountState());
                break;
            case string currency when currency.StartsWith("select_currency1:"):
                order1 = _stateManager.GetOrder1(query.Message.Chat.Id);
                _ = Enum.TryParse(currency.Split(':')[1], out currentCurrency);
                order1.Currency = currentCurrency;
                _stateManager.SetOrder(query.Message.Chat.Id, order1);
                _stateManager.SetState(query.Message.Chat.Id, new PlatformEnterBankingState());
                break;
            //Exchange cash on cash
            case "cash":
                _stateManager.SetState(query.Message.Chat.Id, new CashState());
                break;
            case string currency when currency.StartsWith("select_take_currency1:"):
                Order2 order2 = _stateManager.GetOrder2(query.Message.Chat.Id);
                _ = Enum.TryParse(currency.Split(':')[1], out currentTakeCurrency);
                order2.TakeCurrency = currentTakeCurrency;
                _stateManager.SetOrder(query.Message.Chat.Id, order2);
                _stateManager.SetState(query.Message.Chat.Id, new States.ExchangeStates.CashStates.PlatformEnterAmountState());
                break;
            case string currency when currency.StartsWith("select_currency2:"):
                order2 = _stateManager.GetOrder2(query.Message.Chat.Id);
                _ = Enum.TryParse(currency.Split(':')[1], out currentCurrency);
                order2.Currency = currentCurrency;
                _stateManager.SetOrder(query.Message.Chat.Id, order2);
                _stateManager.SetState(query.Message.Chat.Id, new States.ExchangeStates.CashStates.ConfirmationState());
                break;
            case string confirm when confirm.StartsWith("confirm"):
                string keyword = "confirm";

                int index = confirm.IndexOf(keyword);
                string result = string.Empty;
                if (index != -1)
                    result = confirm[(index + keyword.Length)..];

                switch (result)
                {
                    case "":
                        await _userService.NotifyManagersAsync(_stateManager.GetOrder(query.Message.Chat.Id).ToString());
                        break;
                    case "1":
                        await _userService.NotifyManagersAsync(_stateManager.GetOrder1(query.Message.Chat.Id).ToString());
                        break;
                    case "2":
                        await _userService.NotifyManagersAsync(_stateManager.GetOrder2(query.Message.Chat.Id).ToString());
                        break;
                }
                _stateManager.SetState(query.Message.Chat.Id, new MainMenuState());
                break;
        }

        IBotState currentState = _stateManager.GetState(query.Message.Chat.Id);

        await currentState.Handle(_botClient, query.Message, _stateManager);


        //_messageManager.AddSentMessage(query.Message.Chat.Id, query.Message.Id);
    }
}
