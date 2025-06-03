using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangerBot.Bot.States;

internal class AboutUsState : IBotState
{
    public async Task Handle(ITelegramBotClient bot, Message message, StateManager stateManager)
    {
        long chatId = message.Chat.Id;
        int messageId = message.MessageId;

        var buttons = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("⬅️ Главное меню", "back")]
        ]);

        await bot.EditMessageText(chatId, stateManager.GetGeneralMessageId(chatId),
        @"💱 <b>ABC EXCHANGE</b> — ваш надёжный партнёр в обмене валют!  
Мы работаем в Азии с 2020 года, предлагая:
🔹 Актуальные курсы
🔹 Мгновенное обслуживание
🔹 Полную прозрачность без скрытых комиссий

📊 Наша команда ежедневно отслеживает рынки региона, чтобы вы получали <b>самые выгодные условия</b> там, где находитесь.

🔒 Мы — <b>верифицированный сервис от BaliForum</b>
📌 <a href=""https://t.me/balichatexchange/201725"">Проверить сертификат</a>

🗣 <b>Отзывы наших клиентов:</b>
👉 <a href=""https://t.me/abcxchange/4"">Читать комментарии</a>

📸 Следите за нашими новостями и акциями:
👉 <a href=""https://www.instagram.com/abcexchanges"">Instagram</a>

🌐 Подробнее об услугах и калькулятор курсов:
👉 <a href=""https://abcexchanges.com/"">abcexchanges.com</a>

<b>Наши услуги включают:</b>
✅ Конвертацию валют Азии и мира по выгодным курсам
✅ Выдачу наличных через ATM — быстро и без очередей
✅ Курьерскую доставку наличных в отель или домой
✅ Онлайн-консультации и сделки за пару минут
✅ Индивидуальные условия для крупных сумм и бизнеса
✅ Безопасные транзакции и полная верификация
✅ Оплата счетов и коммунальных услуг

🎯 <b>ABC EXCHANGE</b> — это <b>удобно, быстро и надёжно</b>!",
         parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
         replyMarkup: buttons);
    }
}
