using System.Collections.Generic;

namespace Aurora.Shared.Models;

public class ContentFeed
{
    public ContentItem? VibeOfTheDay { get; set; }
    public List<ContentItem> DailyPicks { get; set; } = new List<ContentItem>();
}
