using ExchangerBot.Bot;
using ExchangerBot.Bot.Database;
using ExchangerBot.Bot.Database.Repositories;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

DatabaseInitializer.Initialize();

string token = config["BotConfig:Token"] ?? throw new UnauthorizedAccessException("Bot token not found.");
TelegramBotClient botClient = new(token);

var dbContext = new AppDbContext();
var userRepository = new UserRepository(dbContext);
var userService = new UserService(userRepository, botClient);
var botService = new BotService(botClient, userService);

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


