using Microsoft.Extensions.Configuration;
using RandomCoffeeBot;
using RandomCoffeeBot.Handlers;
using RandomCoffeeBot.Models;
using SlackNet;

Log.Message("Настройка...");

var settings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build()
    .Get<AppSettings>();

var slackServices = new SlackServiceBuilder()
        .UseApiToken(settings.ApiToken)
        .UseAppLevelToken(settings.AppLevelToken)
        .RegisterSlashCommandHandler(StartCommandHandler.Command, new StartCommandHandler())
        .RegisterSlashCommandHandler(EditInfoCommandHandler.Command, new EditInfoCommandHandler())
        .RegisterSlashCommandHandler(HelpCommandHandler.Command, new HelpCommandHandler())
        .RegisterSlashCommandHandler(StopCommandHandler.Command, new StopCommandHandler())
        .RegisterSlashCommandHandler(ShowInfoCommandHandler.Command, new ShowInfoCommandHandler())
        .RegisterSlashCommandHandler(DebugInfoCommandHandler.Command, new DebugInfoCommandHandler())
        .RegisterEventHandler(ctx => new TextMessageHandler(ctx.ServiceProvider.GetApiClient()))
        .RegisterBlockActionHandler(ReminderHandler.Yes, (ctx => new ReminderHandler(ctx.ServiceProvider.GetApiClient())))
        .RegisterBlockActionHandler(ReminderHandler.No, (ctx => new ReminderHandler(ctx.ServiceProvider.GetApiClient())))
    ;

Log.Message("Подключение...");
var apiClient = slackServices.GetApiClient();
var client = slackServices.GetSocketModeClient();
await client.Connect();

Log.Message("Подключение установлено.");
_ = Task.Run(() => ReminderHandler.RunAsync(apiClient));

await Task.Run(Console.ReadKey);