using Aurora.Shared.Interfaces;
using Aurora.Shared.Models;
using System.Collections.ObjectModel;

namespace Aurora;

public partial class MainPage : ContentPage
{
	private readonly IContentService _contentService;

	private ContentItem? _vibeOfTheDay;
	public ContentItem? VibeOfTheDay
	{
		get => _vibeOfTheDay;
		set
		{
			_vibeOfTheDay = value;
			OnPropertyChanged();
		}
	}

	private ObservableCollection<ContentItem> _dailyPicks = new();
	public ObservableCollection<ContentItem> DailyPicks
	{
		get => _dailyPicks;
		set
		{
			_dailyPicks = value;
			OnPropertyChanged();
		}
	}

	public MainPage(IContentService contentService)
	{
		InitializeComponent();
		_contentService = contentService;
		this.BindingContext = this;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await LoadDataAsync().ConfigureAwait(false);
	}

	private async Task LoadDataAsync()
	{
		try
		{
			var feed = await _contentService.GetDailyContentAsync().ConfigureAwait(false);

			if (feed.VibeOfTheDay != null)
			{
				VibeOfTheDay = feed.VibeOfTheDay;
			}

			if (feed.DailyPicks != null)
			{
				DailyPicks = new ObservableCollection<ContentItem>(feed.DailyPicks);
			}
		}
#pragma warning disable CA1031 // Do not catch general exception types
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				await DisplayAlert("Error", $"Data Load Failed: {ex.Message}", "OK").ConfigureAwait(false);
			});
		}
#pragma warning restore CA1031 // Do not catch general exception types
	}

	/// <summary>
	/// Handles the click event of the reaction button (both Vibe and Daily Picks).
	/// </summary>
	private async void OnReactionButtonClicked(object sender, EventArgs e)
	{
		if (sender is not Button button)
		{
			return;
		}

		// Determine which item was clicked
		ContentItem? item = null;

		if (button == ReactionButton)
		{
			// Vibe of the Day button (bound to Page context)
			item = VibeOfTheDay;
		}
		else
		{
			// Daily Picks button (bound to Item context)
			item = button.BindingContext as ContentItem;
		}

		if (item != null)
		{
			// Optimistic Update - Now triggers UI automatically via INotifyPropertyChanged
			item.UpliftCount++;

			try
			{
				var newCount = await _contentService.ReactToContentAsync(item.Id).ConfigureAwait(false);

				// Confirm count from server (in case of race conditions)
				item.UpliftCount = newCount;
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception ex)
			{
				// Rollback on failure
				item.UpliftCount--;
				System.Diagnostics.Debug.WriteLine($"Reaction failed: {ex.Message}");
			}
#pragma warning restore CA1031 // Do not catch general exception types
		}
	}
	/// <summary>
	/// Handles the click event for sharing a content item.
	/// </summary>
	private async void OnShareClicked(object sender, EventArgs e)
	{
		// Support both Button (old style) and Border (new optically aligned style)
		if (sender is Element element)
		{
			ContentItem? itemToShare = null;

			if (element.BindingContext is ContentItem item)
			{
				// Daily Pick (List Item context)
				itemToShare = item;
			}
			else if (element.BindingContext is MainPage page && page.VibeOfTheDay != null)
			{
				// Vibe Card (Page context)
				itemToShare = page.VibeOfTheDay;
			}

			if (itemToShare != null)
			{
				await Share.Default.RequestAsync(new ShareTextRequest
				{
					Title = itemToShare.Title,
					Text = itemToShare.Title,
					Uri = itemToShare.ArticleUrl
				}).ConfigureAwait(false);
			}
		}
	}

	/// <summary>
	/// Handles the click event for the placeholder comment button.
	/// </summary>
	private async void OnCommentClicked(object sender, EventArgs e)
	{
		await DisplayAlert("Coming Soon", "Comments are not yet available.", "OK").ConfigureAwait(false);
	}
}
