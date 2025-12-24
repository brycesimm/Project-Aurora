using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Aurora.Shared.Models;

/// <summary>
/// Represents a single piece of content (news story, video, etc.) in the Aurora feed.
/// </summary>
public class ContentItem : INotifyPropertyChanged
{
	private int _upliftCount;

	/// <summary>
	/// Occurs when a property value changes.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Gets or sets the unique identifier for the content item.
	/// </summary>
	[JsonPropertyName("id")]
	public required string Id { get; set; }

	/// <summary>
	/// Gets or sets the headline or title of the content.
	/// </summary>
	[JsonPropertyName("title")]
	public required string Title { get; set; }

	/// <summary>
	/// Gets or sets a short summary or snippet of the content.
	/// </summary>
	[JsonPropertyName("snippet")]
	public required string Snippet { get; set; }

	/// <summary>
	/// Gets or sets the URL of the image associated with the content.
	/// </summary>
	[JsonPropertyName("image_url")]
	public required string ImageUrl { get; set; }

	/// <summary>
	/// Gets or sets the URL to the full article or source.
	/// </summary>
	[JsonPropertyName("article_url")]
	public required string ArticleUrl { get; set; }

	/// <summary>
	/// Gets or sets the number of users who found this content uplifting.
	/// </summary>
	[JsonPropertyName("uplift_count")]
	public int UpliftCount
	{
		get => _upliftCount;
		set
		{
			if (_upliftCount != value)
			{
				_upliftCount = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed.</param>
	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
