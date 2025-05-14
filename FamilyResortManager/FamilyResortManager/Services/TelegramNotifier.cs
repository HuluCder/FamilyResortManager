using System.Net.Http;
using System.Threading.Tasks;

public class TelegramNotifier
{
    private readonly string _botToken;
    private readonly string _chatId;
    private readonly HttpClient _httpClient;

    public TelegramNotifier(string botToken, string chatId)
    {
        _botToken = botToken;
        _chatId = chatId;
        _httpClient = new HttpClient();
    }

    public async Task SendMessageAsync(string message)
    {
        var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("chat_id", _chatId),
            new KeyValuePair<string, string>("text", message),
            new KeyValuePair<string, string>("parse_mode", "HTML")
        });

        await _httpClient.PostAsync(url, content);
    }
}