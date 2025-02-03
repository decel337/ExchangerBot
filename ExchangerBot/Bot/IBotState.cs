using Telegram.Bot.Types;
using Telegram.Bot;

namespace ExchangerBot.Bot;

internal interface IBotState
{
    Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager);
}
