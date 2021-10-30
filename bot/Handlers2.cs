using System;
using System.Threading.Tasks;
using bot.Entity;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot
{
    public partial class Handlers
    {
        private async Task BotOnMessageEdited(ITelegramBotClient client, Message editedMessage)
        {
            throw new NotImplementedException();
        }

        private async Task UnknownUpdateHandlerAsync(ITelegramBotClient client, Update update)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnChosenInlineResultReceived(ITelegramBotClient client, ChosenInlineResult chosenInlineResult)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnInlineQueryReceived(ITelegramBotClient client, InlineQuery inlineQuery)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnCallbackQueryReceived(ITelegramBotClient client, CallbackQuery callbackQuery)
        {
            throw new NotImplementedException();
        }
        public async Task getLanguageMessageTextAsync(
            ITelegramBotClient client, string text, Message message)
        {//Assalomu aleykum, Lokatsiyangizni jo'nata olasizmi?
            await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        parseMode: ParseMode.Markdown,
                        text: $"{text}",
                        replyMarkup: MessageBuilder.LocationRequestButton(
                            _storage.GetUserAsync(message.Chat.Id).Result.Language));
        }

        public async Task getMessageTextBackAsync(
            ITelegramBotClient client, string text, Message message)
        {
            await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        parseMode: ParseMode.Markdown,
                        text: $"text",
                        replyMarkup: MessageBuilder.LanguagesButton());
        }
        public async Task getSettingsMessageTextAsync(
            ITelegramBotClient client, string text, Message message)
        {
            await client.SendTextMessageAsync(
                           chatId: message.Chat.Id,
                           parseMode: ParseMode.Markdown,
                           text: $"{text}",
                           replyMarkup: MessageBuilder.LocationRequestButton(
                               _storage.GetUserAsync(message.Chat.Id).Result.Language));
        }
        public async Task getMessageTextMShAsync(ITelegramBotClient client, string Text, Message message)
        {
            await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"{Text}",
                        parseMode: ParseMode.Markdown,
                        replyMarkup: MessageBuilder.MenuShow(
                            _storage.GetUserAsync(message.Chat.Id).Result.Language));
        }

        public string getTimeString(Models.PrayerTime times, Message message)
        {
            if (_storage.GetUserAsync(message.Chat.Id).Result.Language == "English")
            {
                return $" *Fajr*: {times.Fajr}\n*Sunrise*: {times.Sunrise}\n*Dhuhr*: {times.Dhuhr}\n*Asr*: {times.Asr}\n*Maghrib*: {times.Maghrib}\n*Isha*: {times.Isha}\n\n*Method*: {times.CalculationMethod}";
            }
            else if (_storage.GetUserAsync(message.Chat.Id).Result.Language == "Русский")
            {
                return $" *Фажр*: {times.Fajr}\n*Восход*: {times.Sunrise}\n*Зухр*: {times.Dhuhr}\n*Аср*: {times.Asr}\n*Магриб*: {times.Maghrib}\n*Иша*: {times.Isha}\n\n*Method*: {times.CalculationMethod}";
            }
            return $" *Bomdod*: {times.Fajr}\n*Quyosh chiqishi*: {times.Sunrise}\n*Peshin*: {times.Dhuhr}\n*Asr*: {times.Asr}\n*Shom*: {times.Maghrib}\n*Xufton*: {times.Isha}\n\n*Method*: {times.CalculationMethod}";
        }
    }
}