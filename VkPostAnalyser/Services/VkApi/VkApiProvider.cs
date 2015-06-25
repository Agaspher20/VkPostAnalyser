using System.Collections.Generic;
using System.Threading.Tasks;
using VkPostAnalyser.Model;
using VKSharp;
using VKSharp.Data.Api;
using VKSharp.Core.Entities;
using System;

namespace VkPostAnalyser.Services.VkApi
{
    public class VkApiProvider : ISocialApiProvider
    {
        private const int PageSize = 100; // Max count of posts which wall.get returns
        private const string BaseUrl = "http://vk.com";

        public async Task<IEnumerable<PostInfo>> RetrievePostInfosAsync(string userId)
        {
            try
            {
                var api = new VKApi();
                List<PostInfo> postInfos = null;
                int offset = 0;
                bool hasPosts;
                do
                {
                    int pageCount = 0;
                    EntityList<Post> posts = await api.Wall.Get(domain: userId, offset: offset, count: PageSize);
                    if (postInfos == null)
                    {
                        postInfos = new List<PostInfo>(posts.Count);
                    }
                    foreach (var post in posts.Items)
                    {
                        ++pageCount;
                        postInfos.Add(new PostInfo
                        {
                            LikesCount = post.Likes.Count,
                            PostId = post.Id,
                            SignsCount = post.Text.Length,
                            OwnerId = post.OwnerId
                        });
                    }
                    hasPosts = pageCount >= PageSize;
                    offset += pageCount;
                }
                while (hasPosts);

                return postInfos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string BuildPostUrl(PostInfo post)
        {
            return BaseUrl + "/wall" + post.OwnerId + "_" + post.PostId;
        }
    }
}
