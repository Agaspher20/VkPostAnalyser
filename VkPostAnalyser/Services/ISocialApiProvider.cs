using System.Collections.Generic;
using System.Threading.Tasks;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public interface ISocialApiProvider
    {
        Task<IEnumerable<PostInfo>> RetrievePostInfosAsync(string userId);
    }
}
