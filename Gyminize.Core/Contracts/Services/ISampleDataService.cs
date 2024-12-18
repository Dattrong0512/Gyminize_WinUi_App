using Gyminize.Core.Models;

namespace Gyminize.Core.Contracts.Services;

// Remove this class once your pages/features are using your data.
public interface ISampleDataService
{

    Task<IEnumerable<Influencer>> GetInfluencerListDetailsDataAsync();
}
