using SlackNet;
using SlackNet.WebApi;

namespace RandomCoffeeBot.Utils;

public static class Messages
{
    public const string StartMessage =
        """
        Привет!
        Напишите пару слов о себе, это поможет партнёру составить первое впечатление и завязать разговор 
        """;

    public const string InfoReceivedMessage =
        """
        Получилось! 🙌

        Теперь ты — участник встреч Random Coffee ☕️
        Краткая инструкция:

        1) Свою пару для встречи ты будешь узнавать каждый понедельник — сообщение придет в этот чат
        Напиши партнеру, чтобы договориться о встрече или звонке.
        2) В конце недели я спрошу: участвуешь ли ты на следующей неделе.
        3) Если хочешь изменить информацию о себе напиши /edit_info
        4) Полный список команд /help
        """;

    public const string EditInfoMessage =
        "Напишите пару слов о себе, это поможет партнёру " +
        "составить первое впечатление и завязать разговор";

    public const string CommandsMessage =
        """
        Для взаимодействия используй одну из команд: 
        1. /stop - поставить встречи на паузу
        2. /start - возобновить встречи
        3. /edit_info - изменить информацию о себе
        4. /show_info - показать информацию о себе
        """;

    public const string DontUnderstandMessage =
        $"Не понял тебя 🤔 \n {CommandsMessage}";

    public const string PauseMessage =
        """
        Поставил встречи на паузу ⏸️ 
        Ты можешь возобновить встречи в любой момент. Для этого напиши /start
        """;

    public const string InvalidMessage =
        """
        Напиши /start и мы начнём 🚀
        """;

    public const string ResumeMeetingsMessage =
        """
        Получилось, возобновляю встречи! 🚀
        """;

    public const string ReminderMessage =
        """
        Привет!👋
        Встречи Random Coffee продолжаются
        Участвуешь на следующей неделе?
        """;

    public const string ReminderYesResponseMessage =
        """
        Отлично!👍
        Напишу тебе в понедельник.
        """;

    public const string ReminderNoResponseMessage =
        """
        Хорошо, спрошу ещё раз на следующей неделе.
        Если хочешь поставить встречи на паузу напиши /stop
        """;

    public const string ReminderYesEditedMessage = ReminderMessage + "\n Ты ответил: Да";
    public const string ReminderNoEditedMessage = ReminderMessage + "\n Ты ответил: Нет";

    public const string MatchSuccessMessage =
        """
        Знакомься! 🎩
        Твоя пара на эту неделю:
        <@{0}>
        О себе:
        {1}
        """;

    public const string MatchFailMessage =
        """
        Привет! 👋
        Пары на эту неделю распределены. Для тебя, к сожалению, не хватило партнера. Надеюсь получится подобрать кого-то в следующий раз.
        """;


    public static async Task<PostMessageResponse> PostMessageAsync(this ISlackApiClient slack, string text, string channel)
    {
        return await slack.Chat.PostMessage(new Message
        {
            Text = text,
            Channel = channel,
            AsUser = true,
        });
    }
}