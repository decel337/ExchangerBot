using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.States;

namespace ExchangerBot.Bot;

internal class StateManager
{
    private readonly Dictionary<long, IBotState> _userStates = [];
    private readonly Dictionary<long, Order> _usersOrder = [];
    private readonly Dictionary<long, Order1> _usersOrder1 = [];
    public int GeneralMessageId { get; private set; }
    public void SetState(long chatId, IBotState state)
    {
        _userStates[chatId] = state;
    }

    public IBotState GetState(long chatId)
    {
        return _userStates.TryGetValue(chatId, out IBotState? value) ? value : new MainMenuState();
    }

    public void SetOrder(long chatId, Order state)
    {
        _usersOrder[chatId] = state;
    }

    public void SetOrder(long chatId, Order1 state)
    {
        _usersOrder1[chatId] = state;
    }

    public Order GetOrder(long chatId)
    {
        return _usersOrder.TryGetValue(chatId, out Order? value) ? value : throw new ArgumentNullException($"Order not found in {chatId}");
    }

    public Order1 GetOrder1(long chatId)
    {
        return _usersOrder1.TryGetValue(chatId, out Order1? value) ? value : throw new ArgumentNullException($"Order not found in {chatId}");
    }

    public void SetGeneralMessageId(int messageId)
    {
        GeneralMessageId = messageId;
    }
}
