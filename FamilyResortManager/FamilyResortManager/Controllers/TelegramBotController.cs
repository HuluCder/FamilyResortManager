using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/telegram/update")]
[ApiController]
public class TelegramBotController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] JsonElement update)
    {
        var message = update.GetProperty("message");

        var chatId = message.GetProperty("chat").GetProperty("id").GetInt64();
        var text = message.GetProperty("text").GetString();

        if (text == "/start")
        {
            var client = new HttpClient();
            var botToken = "7597481026:AAFTjqO1ijk7osMZfqAyYXAsodCh3iuQKLg";

            var msg = $"Ваш Telegram Chat ID: <code>{chatId}</code>\nСкопируйте и вставьте его в систему для получения уведомлений.";

            var url = $"https://api.telegram.org/bot{botToken}/sendMessage";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("chat_id", chatId.ToString()),
                new KeyValuePair<string, string>("text", msg),
                new KeyValuePair<string, string>("parse_mode", "HTML")
            });

            await client.PostAsync(url, content);
        }

        return Ok();
    }
}