using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class TelegramNotifier
{
    private readonly string _botToken;
    private readonly HttpClient _httpClient;

    public TelegramNotifier(IConfiguration configuration)
    {
        _botToken = configuration["Telegram:BotToken"]!;
        _httpClient = new HttpClient();
    }

    public async Task SendMessageAsync(string chatId, string message)
    {
        if (string.IsNullOrWhiteSpace(chatId)) return;

        var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("chat_id", chatId),
            new KeyValuePair<string, string>("text", message),
            new KeyValuePair<string, string>("parse_mode", "HTML")
        });

        await _httpClient.PostAsync(url, content);
    }
}