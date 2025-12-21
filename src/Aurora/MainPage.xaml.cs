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
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var feed = await _contentService.GetDailyContentAsync();
            
            if (feed.VibeOfTheDay != null)
            {
                VibeOfTheDay = feed.VibeOfTheDay;
            }

            if (feed.DailyPicks != null)
            {
                DailyPicks = new ObservableCollection<ContentItem>(feed.DailyPicks);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            await DisplayAlert("Error", $"Data Load Failed: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Handles the click event of the placeholder reaction button.
    /// In this initial implementation (Story A-01.4), it displays a simple alert.
    /// </summary>
    private void OnReactionButtonClicked(object sender, EventArgs e)
    {
        DisplayAlert("Feedback", "Thank you for your feedback!", "OK");
    }
}