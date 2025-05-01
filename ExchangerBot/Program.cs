using ExchangerBot.Bot;
using ExchangerBot.Bot.Database;
using ExchangerBot.Bot.Database.Repositories;
using ExchangerBot.Bot.OrderService;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace ExchangerBot;
public static class Program
{
    public static OrderService? GlobalOrderService { get; private set; }
    public static void Main(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        DatabaseInitializer.Initialize();
        OrderService orderService = new OrderService();
        GlobalOrderService = orderService;

        string token = config["BotConfig:Token"] ?? throw new UnauthorizedAccessException("Bot token not found.");
        TelegramBotClient botClient = new(token);

        var dbContext = new AppDbContext();
        var userRepository = new UserRepository(dbContext);
        var userService = new UserService(userRepository, botClient);
        var botService = new BotService(botClient, userService, orderService);

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
    }
}