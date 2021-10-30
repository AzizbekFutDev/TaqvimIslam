using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using bot.Entity;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot
{
    public partial class Handlers
    {
        private async Task BotOnMessageReceived(ITelegramBotClient client, Message message)
        {
            if (!await _storage.ExistsAsync(message.Chat.Id))
            {
                var user = new BotUser(
                    chatId: message.Chat.Id,
                    username: message.From.Username,
                    fullname: $"{message.From.FirstName} {message.From.LastName}",
                    latitude: 0,
                    longitude: 0,
                    address: string.Empty,
                    language: string.Empty);

                var result = await _storage.InsertUserAsync(user);

                if (result.IsSuccess)
                {
                    _logger.LogInformation($"New user added: {message.Chat.Id}");
                }
            }
            else
            {
                _logger.LogInformation($"User exists");
            }

            var language = "";
            // var User = new BotUser(message.Chat.Id, message.From.Username, message.From.FirstName + message.From.LastName, message.Location.Longitude, message.Location.Latitude, string.Empty);
            if (message.Type == MessageType.Location && message.Location != null)
            {
                if (await _storage.ExistsAsync(message.Chat.Id))
                {
                    var user = new BotUser(
                            chatId: message.Chat.Id,
                            username: message.From.Username,
                            fullname: $"{message.From.FirstName} {message.From.LastName}",
                            latitude: message.Location.Latitude,
                            longitude: message.Location.Longitude,
                            address: string.Empty,
                            language: _storage.GetUserAsync(message.Chat.Id).Result.Language);
                    var result = await _storage.UpdateUserAsync(user);

                    if (result.IsSuccess)
                    {
                        _logger.LogInformation($"User exists");
                    }
                    else
                    {
                        _logger.LogInformation($"User added");
                    }

                }
                if (_storage.GetUserAsync(message.Chat.Id).Result.Language == "English")
                {
                    await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "Select day",
                        parseMode: ParseMode.Markdown,
                        replyMarkup: MessageBuilder.MenuShow(_storage.GetUserAsync(message.Chat.Id).Result.Language));
                }
                else if (_storage.GetUserAsync(message.Chat.Id).Result.Language == "Русский")
                {
                    await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "Выберите день",
                        parseMode: ParseMode.Markdown,
                        replyMarkup: MessageBuilder.MenuShow(_storage.GetUserAsync(message.Chat.Id).Result.Language));
                }
                else if (_storage.GetUserAsync(message.Chat.Id).Result.Language == "O'zbek")
                {
                    await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "Kunni tanlang",
                        parseMode: ParseMode.Markdown,
                        replyMarkup: MessageBuilder.MenuShow(_storage.GetUserAsync(message.Chat.Id).Result.Language));
                }
            }
            switch (message.Text)
            {
                case "/start":
                    await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            parseMode: ParseMode.Markdown,
                            text: "English | O'zbek | Русский?",
                            replyMarkup: MessageBuilder.LanguagesButton());

                    await client.DeleteMessageAsync(
                        chatId: message.Chat.Id,
                        messageId: message.MessageId); break;
                case "English":

                    var userEnglish = await _storage.GetUserAsync(message.Chat.Id);
                    userEnglish.Language = "English";
                    await _storage.UpdateUserAsync(userEnglish);
                    await getLanguageMessageTextAsync(client, "Assalomu aleykum, Can you share your location?", message); break; break;
                    break;

                case "Русский":
                    var userRussian = await _storage.GetUserAsync(message.Chat.Id);
                    userRussian.Language = "Русский";
                    await _storage.UpdateUserAsync(userRussian);
                    await getLanguageMessageTextAsync(client, "Ассалому алейкум, Можете отправить вашу локацию?", message); break; break;
                case "O'zbek":
                    var userOzbek = await _storage.GetUserAsync(message.Chat.Id);
                    userOzbek.Language = "O'zbek";
                    await _storage.UpdateUserAsync(userOzbek);
                    await getLanguageMessageTextAsync(client, "Assalomu aleykum, Lokatsiyangizni jo'nata olasizmi?", message); break;
                case "Back":
                    await getMessageTextBackAsync(client, "Select language", message); break;
                case "Назад":
                    await getMessageTextBackAsync(client, "Выберите язык", message); break;
                case "Orqaga":
                    await getMessageTextBackAsync(client, "Tilni tanlang", message); break;
                case "Today":
                    {
                        var result = await _cache.GetOrUpdatePrayerTimeAsync(
                            message.Chat.Id, _storage.GetUserAsync(message.Chat.Id).Result.Latitude,
                            _storage.GetUserAsync(message.Chat.Id).Result.Longitude);
                        var times = result.prayerTime;
                        await getMessageTextMShAsync(client, getTimeString(times, message), message);
                    }
                    break;
                case "Сегодняшний":
                    {
                        var result = await _cache.GetOrUpdatePrayerTimeAsync(
                            message.Chat.Id, _storage.GetUserAsync(message.Chat.Id).Result.Latitude,
                             _storage.GetUserAsync(message.Chat.Id).Result.Longitude);
                        var times = result.prayerTime;
                        await getMessageTextMShAsync(client, getTimeString(times, message), message);
                    }
                    break;
                case "Bugungi":
                    {
                        var result = await _cache.GetOrUpdatePrayerTimeAsync(
                            message.Chat.Id, _storage.GetUserAsync(message.Chat.Id).Result.Latitude,
                            _storage.GetUserAsync(message.Chat.Id).Result.Longitude);
                        var times = result.prayerTime;
                        await getMessageTextMShAsync(client, getTimeString(times, message), message);
                    }
                    break;
                case "Settings":
                    await getSettingsMessageTextAsync(client, "Do you want change your location?", message); break;
                case "Настройки":
                    await getSettingsMessageTextAsync(client, "Хотите изменить локацию?", message); break;
                case "Sozlamalar":
                    await getSettingsMessageTextAsync(client, "Lokatsiyangizni o'zgartirmoqchimisiz?", message); break;
            }
        }
    }
}