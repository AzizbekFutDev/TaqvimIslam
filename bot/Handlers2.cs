using System;
using System.IO;
using System.Threading.Tasks;
using bot.Entity;
using bot.HttpClients;
using bot.Services;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Topten.RichTextKit;

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
        private async Task BotOnMessageReceived(ITelegramBotClient client, Message message)
        {
            if(message.Type == MessageType.Location && message.Location != null)
            {
                var result = await _storage.GetOrUpdatePrayerTimeAsync(message.Chat.Id, message.Location.Latitude, message.Location.Longitude);
                var times = result.prayerTime;
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    "Select day",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: MessageBuilder.DateButtonEn());

                switch(message.Text)
                {
                    case "Today": 
                    await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        parseMode: ParseMode.Markdown,
                        text: getTimeString(times),
                        replyMarkup: MessageBuilder.LocationRequestButtonEn()); break;
                }
            }
            switch(message.Text)
            {
                case "/start": 
                    await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            parseMode: ParseMode.Markdown,
                            text: "English | O'zbek | Русский?",
                            replyMarkup: MessageBuilder.LanguagesButton());   
                    if(!await _storage.ExistsAsync(message.Chat.Id))
                    {
                        var user = new BotUser(
                            chatId: message.Chat.Id,
                            username: message.From.Username,
                            fullname: $"{message.From.FirstName} {message.From.LastName}",
                            latitude: message.Location.Latitude,
                            longitude: message.Location.Longitude,
                            address: string.Empty);

                        var result = await _storage.InsertUserAsync(user);

                        if(result.IsSuccess)
                        {
                            _logger.LogInformation($"New user added: {message.Chat.Id}");
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"User exists");
                    }
                    await client.DeleteMessageAsync(
                        chatId: message.Chat.Id,
                        messageId: message.MessageId); break;
                case "English":
                {
                    await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        parseMode: ParseMode.Markdown,
                        text: "Assalomu aleykum, Can you share your location?",
                        replyMarkup: MessageBuilder.LocationRequestButtonEn());
                }; break;

                case "Русский":
                {
                    await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        parseMode: ParseMode.Markdown,
                        text: "Ассалому алейкум, Можете отправить вашу локацию?",
                        replyMarkup: MessageBuilder.LocationRequestButtonRu());
                }; break;
                case "O'zbek":
                {
                    await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        parseMode: ParseMode.Markdown,
                        text: "Assalomu aleykum, Lokatsiyangizni jo'nata olasizmi?",
                        replyMarkup: MessageBuilder.LocationRequestButtonUz());
                    
                }; break;
                case "Back":
                await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            parseMode: ParseMode.Markdown,
                            text: "Select language?",
                            replyMarkup: MessageBuilder.LanguagesButton()); break;
                case "Назад":
                await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            parseMode: ParseMode.Markdown,
                            text: "Выберите язык?",
                            replyMarkup: MessageBuilder.LanguagesButton()); break;
                case "Orqaga":
                await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            parseMode: ParseMode.Markdown,
                            text: "Tilni tanglang?",
                            replyMarkup: MessageBuilder.LanguagesButton()); break; 
                // case "Today":
                // var res = await _cache.GetOrUpdatePrayerTimeAsync(message.Chat.Id, message.Location.Latitude, message.Location.Longitude);
                // var times = res.prayerTime;
                // await client.SendTextMessageAsync(
                //     chatId: message.Chat.Id,
                //     text: getTimeString(times),
                //     replyMarkup: MessageBuilder.LocationRequestButtonEn()); break;
            }    
        }
        public Stream getImageFile(Models.PrayerTime times)
        {
            var text = getTimeString(times);
            using (var surface = SKSurface.Create(new SKImageInfo(500, 500)))
            {
                Draw(surface, text);
                using var image = surface.Snapshot();

                using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                return data.AsStream();
            }
        }

        public void Draw(SKSurface surface, string text)
        {
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.LightGray);

            // Find the canvas bounds
            var canvasBounds = canvas.DeviceClipBounds;

            // Create the text block
            var tb = new TextBlock();

            // Configure layout properties
            tb.MaxWidth = canvasBounds.Width * 0.8f;
            tb.Alignment = TextAlignment.Left;

            var style = new Style()
            {
                FontSize = 24
            };

            // Add text to the text block
            tb.AddText(text, style);

            // Paint the text block
            tb.Paint(canvas, new SKPoint(canvasBounds.Width * 0.1f, canvasBounds.Height * 0.1f));
        }
        public string getTimeString(Models.PrayerTime times)
            => $" *Fajr*: {times.Fajr}\n*Sunrise*: {times.Sunrise}\n*Dhuhr*: {times.Dhuhr}\n*Asr*: {times.Asr}\n*Maghrib*: {times.Maghrib}\n*Isha*: {times.Isha}\n*Midnight*: {times.Midnight}\n\n*Method*: {times.CalculationMethod}";
    

        // private string prayertimeF_En(Models.PrayerTime times)
        //     => $"Fajr: {times.Fajr}\nSunrise: {times.Sunrise}\nDhuhr: {times.Dhuhr}\nAsr: {times.Asr}\nMaghrib: {times.Maghrib}\nIsha: {times.Isha}\n\nMethod: {times.CalculationMethod}";
        // private string prayertimeF_Uz(Models.PrayerTime times)
        //     => $"Bomdod: {times.Fajr}\nQuyosh chiqishi: {times.Sunrise}\nPeshin: {times.Dhuhr}\nAsr: {times.Asr}\nShom: {times.Maghrib}\nXufton: {times.Isha}\n\nMethod: {times.CalculationMethod}";
        // private string prayertimeF_Ru(Models.PrayerTime times)
        //     => $"Бомдод: {times.Fajr}\nВосход: {times.Sunrise}\nПешин: {times.Dhuhr}\nАср: {times.Asr}\nШом: {times.Maghrib}\nХуфтон: {times.Isha}\n\nMethod: {times.CalculationMethod}";
    
    }
}