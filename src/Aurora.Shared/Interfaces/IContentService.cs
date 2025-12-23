using System.Threading.Tasks;
using Aurora.Shared.Models;

namespace Aurora.Shared.Interfaces;

/// <summary>
/// Defines the contract for retrieving content for the application.
/// </summary>
public interface IContentService
{
	/// <summary>
	/// Asynchronously retrieves the daily content feed, including the Vibe of the Day and Daily Picks.
	/// </summary>
	/// <returns>A task representing the asynchronous operation, containing the <see cref="ContentFeed"/>.</returns>
	Task<ContentFeed> GetDailyContentAsync();
}
