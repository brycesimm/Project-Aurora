using Aurora.Models;

namespace Aurora;

public partial class MainPage : ContentPage
{
    public List<ContentItem> DailyPicks { get; set; }

    public MainPage()
    {
        InitializeComponent();

        // Populate with mock data (5-10 items as per AC 2)
        DailyPicks = new List<ContentItem>
        {
            new ContentItem { Id = Guid.NewGuid().ToString(), Title = "Innovative Farming Techniques Boost Global Food Supply", Snippet = "New agricultural methods are leading to unprecedented crop yields.", ImageUrl = "https://picsum.photos/seed/farm/800/600", ArticleUrl = "https://example.com/farm" },
            new ContentItem { Id = Guid.NewGuid().ToString(), Title = "Community Project Transforms Urban Blight into Green Space", Snippet = "Local residents collaborate to create a vibrant new park.", ImageUrl = "https://picsum.photos/seed/park/800/600", ArticleUrl = "https://example.com/park" },
            new ContentItem { Id = Guid.NewGuid().ToString(), Title = "Breakthrough in Renewable Energy Promises Cleaner Future", Snippet = "Scientists develop highly efficient, cost-effective solar technology.", ImageUrl = "https://picsum.photos/seed/solar/800/600", ArticleUrl = "https://example.com/solar" },
            new ContentItem { Id = Guid.NewGuid().ToString(), Title = "Students Launch Initiative to Mentor Underprivileged Youth", Snippet = "A powerful program connecting young leaders with aspiring students.", ImageUrl = "https://picsum.photos/seed/mentor/800/600", ArticleUrl = "https://example.com/mentor" },
            new ContentItem { Id = Guid.NewGuid().ToString(), Title = "Local Artist Collective Beautifies City with Murals", Snippet = "Public art installations are fostering community pride and engagement.", ImageUrl = "https://picsum.photos/seed/art/800/600", ArticleUrl = "https://example.com/art" },
            new ContentItem { Id = Guid.NewGuid().ToString(), Title = "Volunteers Rally to Support Local Animal Shelter", Snippet = "Dedicated individuals provide care and find homes for countless pets.", ImageUrl = "https://picsum.photos/seed/shelter/800/600", ArticleUrl = "https://example.com/shelter" },
            new ContentItem { Id = Guid.NewGuid().ToString(), Title = "Tech Startup Develops App to Aid Elderly Independence", Snippet = "New digital tools are empowering seniors to live fuller lives at home.", ImageUrl = "https://picsum.photos/seed/tech/800/600", ArticleUrl = "https://example.com/tech" },
            new ContentItem { Id = Guid.NewGuid().ToString(), Title = "Historic Preservation Efforts Save Beloved Landmark", Snippet = "A community-led campaign secures the future of an iconic building.", ImageUrl = "https://picsum.photos/seed/landmark/800/600", ArticleUrl = "https://example.com/landmark" }
        };

        this.BindingContext = this;
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
