using System.Collections.Generic;
using System.Threading.Tasks;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public interface ISocialApiProvider
    {
        Task<IList<PostInfo>> RetrievePostInfosAsync(string userAlias);

        string BuildPostUrl(UserReport report, PostInfo post);
    }
}
