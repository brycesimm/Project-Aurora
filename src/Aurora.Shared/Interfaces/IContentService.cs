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

	/// <summary>
	/// Asynchronously submits a positive reaction for a specific content item.
	/// </summary>
	/// <param name="contentId">The unique identifier of the content item.</param>
	/// <returns>A task representing the asynchronous operation, containing the updated reaction count.</returns>
	Task<int> ReactToContentAsync(string contentId);
}
