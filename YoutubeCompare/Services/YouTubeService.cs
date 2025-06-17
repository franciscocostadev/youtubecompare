using System.Net.Http;
using System.Text.Json;
using YoutubeCompare.Models;
using Microsoft.Extensions.Configuration;

namespace YoutubeCompare.Services
{
    public class YouTubeService
    {
        private readonly string _apiKey;
        private readonly HttpClient _http;

        public YouTubeService(IConfiguration config)
        {
            _apiKey = config["YouTube:ApiKey"] ?? throw new ArgumentNullException("API Key não encontrada.");
            _http = new HttpClient();
        }

        public async Task<YouTubeChannel?> GetChannelByNameAsync(string name)
        {
            var searchUrl = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={Uri.EscapeDataString(name)}&type=channel&key={_apiKey}";
            var searchResponse = await _http.GetStringAsync(searchUrl);
            var searchJson = JsonDocument.Parse(searchResponse);

            var items = searchJson.RootElement.GetProperty("items");

            if (items.GetArrayLength() == 0)
                return null;

            var channelId = items[0].GetProperty("snippet").GetProperty("channelId").GetString();

            if (string.IsNullOrEmpty(channelId)) return null;

            var channelUrl = $"https://www.googleapis.com/youtube/v3/channels?part=snippet,statistics&id={channelId}&key={_apiKey}";
            var channelResponse = await _http.GetStringAsync(channelUrl);
            var channelJson = JsonDocument.Parse(channelResponse);

            var item = channelJson.RootElement.GetProperty("items")[0];
            var snippet = item.GetProperty("snippet");
            var stats = item.GetProperty("statistics");

            return new YouTubeChannel
            {
                ChannelId = channelId,
                Title = snippet.GetProperty("title").GetString() ?? "",
                Description = snippet.GetProperty("description").GetString() ?? "",
                ThumbnailUrl = snippet.GetProperty("thumbnails").GetProperty("default").GetProperty("url").GetString() ?? "",
                PublishedAt = DateTime.Parse(snippet.GetProperty("publishedAt").GetString() ?? DateTime.MinValue.ToString()),
                SubscriberCount = ulong.Parse(stats.GetProperty("subscriberCount").GetString() ?? "0"),
                VideoCount = ulong.Parse(stats.GetProperty("videoCount").GetString() ?? "0"),
                ViewCount = ulong.Parse(stats.GetProperty("viewCount").GetString() ?? "0")
            };
        }
    }
}
