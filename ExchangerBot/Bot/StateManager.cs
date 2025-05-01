using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.States;

namespace ExchangerBot.Bot;

internal class StateManager
{
    private readonly Dictionary<long, IBotState> _userStates = [];
    private readonly Dictionary<long, int> _generalMessages = [];
    private readonly Dictionary<long, IOrder> _usersOrder = [];
    private int _countOfOrder = int.Parse(File.ReadAllText("count.txt"));
    private static readonly object fileLock = new();
    public int CountOfOrder => _countOfOrder;
    public void SetState(long chatId, IBotState state)
    {
        _userStates[chatId] = state;
    }

    public IBotState GetState(long chatId)
    {
        return _userStates.TryGetValue(chatId, out IBotState? value) ? value : new MainMenuState();
    }

    public void SetOrder(long chatId, IOrder state)
    {
        _usersOrder[chatId] = state;
    }

    public IOrder GetOrder(long chatId)
    {
        return _usersOrder.TryGetValue(chatId, out IOrder? value) ? value : throw new ArgumentNullException($"Order not found in {chatId}");
    }

    public void RemoveOrder(long chatId)
    {
        _usersOrder.Remove(chatId);
    }

    public void SetGeneralMessageId(long chatId, int messageId)
    {
        _generalMessages[chatId] = messageId;
    }

    public int GetGeneralMessageId(long chatId)
    {
        return _generalMessages[chatId];
    }

    public void IncrementCountOfOrder()
    {
        try
        {
            lock (fileLock)
            {
                _countOfOrder++;
                File.WriteAllText("count.txt", _countOfOrder.ToString());
            }
        }
        catch
        {
            Console.WriteLine("PROBLEM WITH FILE COUNT!!!");
        }
    }
}
