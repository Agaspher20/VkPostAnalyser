using System.Collections.Generic;
using System.Threading.Tasks;
using VkPostAnalyser.Domain.Model;

namespace VkPostAnalyser.Services
{
    public interface ISocialApiProvider
    {
        Task<IList<PostInfo>> RetrievePostInfosAsync(int userId, ApplicationUser author);

        string BuildPostUrl(UserReport report, PostInfo post);
    }
}
