using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoutubeCompare.Models;
using YoutubeCompare.Services;

namespace YoutubeCompare.Pages
{
    public class CompareModel : PageModel
    {
        private readonly YouTubeService _youTubeService;

        public CompareModel(YouTubeService youTubeService)
        {
            _youTubeService = youTubeService;
        }

        [BindProperty]
        public string Channel1 { get; set; } = string.Empty;

        [BindProperty]
        public string Channel2 { get; set; } = string.Empty;

        public YouTubeChannel? Canal1 { get; set; }
        public YouTubeChannel? Canal2 { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            Canal1 = await _youTubeService.GetChannelByNameAsync(Channel1);
            Canal2 = await _youTubeService.GetChannelByNameAsync(Channel2);

            return Page();
        }
    }
}