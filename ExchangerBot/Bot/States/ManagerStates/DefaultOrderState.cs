using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States.ManagerStates;

internal class DefaultOrderState : IFormBotState
{
    public Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        return Task.CompletedTask;
    }
}
