using Aurora.Models;

namespace Aurora;

public partial class MainPage : ContentPage
{
    public List<DailyPick> DailyPicks { get; set; }

    public MainPage()
    {
        InitializeComponent();

        // Populate with mock data (5-10 items as per AC 2)
        DailyPicks = new List<DailyPick>
        {
            new DailyPick { Title = "Innovative Farming Techniques Boost Global Food Supply", Snippet = "New agricultural methods are leading to unprecedented crop yields." },
            new DailyPick { Title = "Community Project Transforms Urban Blight into Green Space", Snippet = "Local residents collaborate to create a vibrant new park." },
            new DailyPick { Title = "Breakthrough in Renewable Energy Promises Cleaner Future", Snippet = "Scientists develop highly efficient, cost-effective solar technology." },
            new DailyPick { Title = "Students Launch Initiative to Mentor Underprivileged Youth", Snippet = "A powerful program connecting young leaders with aspiring students." },
            new DailyPick { Title = "Local Artist Collective Beautifies City with Murals", Snippet = "Public art installations are fostering community pride and engagement." },
            new DailyPick { Title = "Volunteers Rally to Support Local Animal Shelter", Snippet = "Dedicated individuals provide care and find homes for countless pets." },
            new DailyPick { Title = "Tech Startup Develops App to Aid Elderly Independence", Snippet = "New digital tools are empowering seniors to live fuller lives at home." },
            new DailyPick { Title = "Historic Preservation Efforts Save Beloved Landmark", Snippet = "A community-led campaign secures the future of an iconic building." }
        };

        this.BindingContext = this;
    }
}
