# Random Coffee Bot
Random Coffee — это слак бот, который помогает людям знакомиться и общаться с новыми собеседниками

Вот как это работает простыми словами:

- Ты регистрируешься в боте и заполняешь пару слов о себе

- Каждую неделю бот автоматически подбирает тебе нового собеседника из других участников.

- Бот присылает тебе контакты собеседника
    
- Ты договариваешься о встрече — это может быть онлайн-звонок или встреча вживую

- Общаешься, знакомишься, делишься опытом — всё это без неловкости, потому что собеседник тоже ждёт знакомства и готов общаться

# Настройка
1. [Создать новое Slack приложение](https://api.slack.com/quickstart#creating)
2. [Запросить нужные разрешения](https://api.slack.com/quickstart#scopes) `commands`, `history`, `im:read`, `im:write`, `users:read`
3. [Установить приложение](https://api.slack.com/quickstart#installing) и скопировать токен OAuth со страницы `OAuth & Permissions` в файл `appsettings.json` для поля ApiToken.
4. Активировать [Socket Mode](https://api.slack.com/apis/socket-mode#toggling) и [сгенерировать AppLevelToken](https://api.slack.com/apis/socket-mode#token) после этого добавить его в `appsettings.json`
5. Активировать `Events Mode`. Для этого зайти в раздел `EventSubscribtions` и перевести тоггл  `Enable Events` в состояние On. После этого ниже в разделах `Subscribe to bot events` и `Subscribe to events on behalf of users` добавить ивент message.im
6. Добавить команды . Для этого зайти в раздел `Slash commands` и добавить следующие команды

| Name        | Description                  |
| ----------- | ---------------------------- |
| /start      | Начать встречи               |
| /edit_info  | Обновить инфо о себе         |
| /help       | Посмотреть все команды       |
| /stop       | Поставить встречи на паузу   |
| /show_info  | Посмотреть инфо о себе       |
| /debug_info | Информация для разработчиков |


