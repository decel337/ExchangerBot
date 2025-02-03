using ExchangerBot.Bot;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

string token = config["BotConfig:Token"] ?? throw new UnauthorizedAccessException("Bot token not found.");
TelegramBotClient botClient = new(token);
var botService = new BotService(botClient);

botClient.StartReceiving(async (bot, update, token) =>
{
    await botService.HandleUpdate(update);
},
async (bot, exception, token) =>
{
    Console.WriteLine($"Error: {exception.Message}");
    await Task.CompletedTask;
});

Console.WriteLine("Bot started. Press Enter for exit.");
Console.ReadLine();


