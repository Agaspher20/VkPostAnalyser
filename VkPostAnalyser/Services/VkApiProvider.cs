using System.Collections.Generic;
using System.Threading.Tasks;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public class VkApiProvider : ISocialApiProvider
    {
        public async Task<IEnumerable<PostInfo>> RetrievePostInfosAsync(string userId)
        {
            Task<IEnumerable<PostInfo>> postsTask = Task.Run<IEnumerable<PostInfo>>(() => new List<PostInfo>
            {
                new PostInfo
                {
                    LikesCount = 384,
                    PostId = "3",
                    SignsCount = 1024,
                    UserId = userId
                },
                new PostInfo
                {
                    LikesCount = 384,
                    PostId = "7",
                    SignsCount = 1024,
                    UserId = userId
                }
            });
            IEnumerable<PostInfo> postInfos = await postsTask;
            return postInfos;
        }
    }
}
