using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkPostAnalyser.Model;
using VkPostAnalyser.Services.Authentication;
using VkPostAnalyser.Services.VkApi.AuthProvider;
using VKSharp;
using VKSharp.Core.Entities;
using VKSharp.Data.Api;

namespace VkPostAnalyser.Services.VkApi
{
    public class VkApiProvider : ISocialApiProvider
    {
        private const int PageSize = 100; // Max count of posts which wall.get returns

        public async Task<IList<PostInfo>> RetrievePostInfosAsync(string userAlias, ApplicationUser author)
        {
            try
            {
                var api = new VKApi();
                if (author != null)
                {
                    api.AddToken(new VKToken(author.Token, userId: author.Id));
                }
                List<PostInfo> postInfos = null;
                int offset = 0;
                bool hasPosts;
                do
                {
                    int pageCount = 0;
                    EntityList<Post> posts = await api.Wall.Get(domain: userAlias, offset: offset, count: PageSize);
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

        public string BuildPostUrl(UserReport report, PostInfo post)
        {
            return VkConstants.VkBaseUrl + "/wall" + report.UserId.Value + "_" + post.PostId;
        }
    }
}
