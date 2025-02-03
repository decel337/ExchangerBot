using Telegram.Bot;

namespace ExchangerBot.Bot;

internal class MessageManager
{
    private readonly Dictionary<long, List<int>> _sentMessages = [];

    public void AddSentMessage(long chatId, int messageId)
    {
        if (!_sentMessages.TryGetValue(chatId, out List<int>? messages))
        {
            messages = [];
            _sentMessages[chatId] = messages;
        }

        messages.Add(messageId);
    }

    public async Task DeleteSentMessages(ITelegramBotClient bot, long chatId)
    {
        if (_sentMessages.TryGetValue(chatId, out List<int>? messages))
        {
            foreach (var messageId in messages)
            {
                try
                {
                    await bot.DeleteMessage(chatId, messageId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error delete messages: {ex.Message}");
                }
            }

            messages.Clear();
        }
    }
}
