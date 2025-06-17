using Microsoft.AspNetCore.Mvc;

namespace YoutubeCompare.Models
{
    public class YouTubeChannel
    {
        public string ChannelId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public ulong SubscriberCount { get; set; }
        public ulong VideoCount { get; set; }
        public ulong ViewCount { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
