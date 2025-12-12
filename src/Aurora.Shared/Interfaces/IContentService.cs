using System.Threading.Tasks;
using Aurora.Shared.Models;

namespace Aurora.Shared.Interfaces;

public interface IContentService
{
    Task<ContentFeed> GetDailyContentAsync();
}
