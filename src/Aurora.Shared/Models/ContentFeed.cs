using System.Collections.Generic;

namespace Aurora.Shared.Models;

/// <summary>
/// Represents the daily content payload containing the "Vibe" and the daily picks.
/// </summary>
public class ContentFeed
{
	/// <summary>
	/// Gets or sets the featured "Vibe of the Day" content item.
	/// </summary>
	public ContentItem? VibeOfTheDay { get; set; }

	/// <summary>
	/// Gets or sets the list of curated daily picks.
	/// </summary>
	public List<ContentItem> DailyPicks { get; set; } = new List<ContentItem>();
}
