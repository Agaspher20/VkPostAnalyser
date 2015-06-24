using System.Collections.Generic;
using System.Linq;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public interface IRepository
    {
        IQueryable<PostInfo> PostInfos { get; }

        void SavePosts(IEnumerable<PostInfo> postInfos);
    }
}
