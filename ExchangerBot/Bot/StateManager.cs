using ExchangerBot.Bot.Models;
using ExchangerBot.Bot.States;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace ExchangerBot.Bot;

internal class StateManager
{
    private readonly Dictionary<long, IBotState> _userStates = [];
    private readonly Dictionary<long, Order> _usersOrder = [];
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

    public Order GetOrder(long chatId)
    {
        return _usersOrder.TryGetValue(chatId, out Order? value) ? value : throw new ArgumentNullException($"Order not found in {chatId}");
    }
}
