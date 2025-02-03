using ExchangerBot.Bot.States;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using ExchangerBot.Bot.States.ExchangeStates;

namespace ExchangerBot.Bot;

internal class BotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly StateManager _stateManager;
    private readonly MessageManager _messageManager;

    public BotService(ITelegramBotClient botClient)
    {
        _botClient = botClient;
        _stateManager = new StateManager();
        _messageManager = new MessageManager();
    }

    public async Task HandleUpdate(Update update)
    {
        if (update.Type == UpdateType.Message && update.Message?.Text == "/start")
        {
            await ShowMainMenu(update.Message);
        }
        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
        {
            await HandleCallbackQuery(update.CallbackQuery);
        }
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
            //Exhange
            case "crypto":
                _stateManager.SetState(query.Message.Chat.Id, new CryptoState());
                break;
        }

        IBotState currentState = _stateManager.GetState(query.Message.Chat.Id);

        if(query.Message is not null)
            await currentState.Handle(_botClient, query.Message, _stateManager);

        //_messageManager.AddSentMessage(query.Message.Chat.Id, query.Message.Id);
    }
}
