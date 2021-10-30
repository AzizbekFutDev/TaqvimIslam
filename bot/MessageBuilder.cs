using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot
{
    public class MessageBuilder
    {
        public static ReplyKeyboardMarkup DateButtonEn()
            => new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "Today" },
                                    new KeyboardButton(){ Text = "Back" },
                                    new KeyboardButton(){ Text= "Settings"},
                                }
                            },
                ResizeKeyboard = true
            };

        public static ReplyKeyboardMarkup MenuShow(string language)
        {
            if (language == "O'zbek")
            {
                return new ReplyKeyboardMarkup()
                {
                    Keyboard = new List<List<KeyboardButton>>()
                                {
                                    new List<KeyboardButton>()
                                    {
                                        new KeyboardButton(){ Text = "Bugungi"},
                                        new KeyboardButton(){ Text= "Sozlamalar"},
                                        new KeyboardButton(){ Text = "Orqaga"},
                                    }
                                },
                    ResizeKeyboard = true
                };
            }
            else if (language == "Русский")
            {
                return new ReplyKeyboardMarkup()
                {
                    Keyboard = new List<List<KeyboardButton>>()
                                {
                                    new List<KeyboardButton>()
                                    {
                                        new KeyboardButton(){ Text = "Сегодняшний"},
                                        new KeyboardButton(){ Text= "Настройки"},
                                        new KeyboardButton(){ Text = "Назад"}
                                    }
                                },
                    ResizeKeyboard = true
                };
            }
            return new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                                {
                                    new List<KeyboardButton>()
                                    {
                                        new KeyboardButton(){ Text = "Today"},
                                        new KeyboardButton(){ Text= "Settings"},
                                        new KeyboardButton(){ Text = "Back"}
                                    }
                                },
                ResizeKeyboard = true
            };
        }
        public static ReplyKeyboardMarkup LocationRequestButton(string language)
        {
            if (language == "O'zbek")
            {
                return new ReplyKeyboardMarkup()
                {
                    Keyboard = new List<List<KeyboardButton>>()
                                {
                                    new List<KeyboardButton>()
                                    {
                                        new KeyboardButton(){ Text = "Lokatsiya jo'natish", RequestLocation = true },
                                        new KeyboardButton(){ Text = "Orqaga"},
                                    }
                                },
                    ResizeKeyboard = true
                };
            }
            else if (language == "Русский")
            {
                return new ReplyKeyboardMarkup()
                {
                    Keyboard = new List<List<KeyboardButton>>()
                                {
                                    new List<KeyboardButton>()
                                    {
                                        new KeyboardButton(){ Text = "Поделиться локацией", RequestLocation = true },
                                        new KeyboardButton(){ Text = "Назад"}
                                    }
                                },
                    ResizeKeyboard = true
                };
            }
            return new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "Share location", RequestLocation = true },
                                    new KeyboardButton(){ Text = "Back"}
                                }
                            },
                ResizeKeyboard = true
            };
        }
        public static ReplyKeyboardMarkup LanguagesButton()
            => new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "English" },
                                    new KeyboardButton(){ Text = "O'zbek" },
                                    new KeyboardButton(){ Text = "Русский" }
                                }
                            },
                ResizeKeyboard = true
            };
    }
}