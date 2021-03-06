﻿using System.Collections.Generic;
using System.Threading.Tasks;
using VkPostAnalyser.Domain.Model;
using VKSharp;
using VKSharp.Core.Entities;
using VKSharp.Data.Api;

namespace VkPostAnalyser.Domain.Services.VkApi
{
    public class VkApiProvider : ISocialApiProvider
    {
        public async Task<IList<PostInfo>> RetrievePostInfosAsync(int userId, ApplicationUser author)
        {
            var api = new VKApi();
            if (author != null)
            {
                api.AddToken(new VKToken(author.Token, userId: author.Id));
            }
            List<PostInfo> postInfos = null;
            int offset = 0, requestsCount = 1;
            bool hasPosts;
            do
            {
                int pageCount = 0;
                EntityList<Post> posts = await api.Wall.Get(ownerId: userId, offset: offset, count: VkConstants.MaxWallPageSize);
                ++requestsCount;
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
                hasPosts = pageCount >= VkConstants.MaxWallPageSize;
                offset += pageCount;
                if (requestsCount == VkConstants.MaxRequestsCountPerSecond)
                {
                    requestsCount = 0;
                    await Task.Delay(1000);
                }
            }
            while (hasPosts);

            return postInfos;
        }

        public string BuildPostUrl(UserReport report, PostInfo post)
        {
            return VkConstants.VkBaseUrl + "/wall" + report.UserId + "_" + post.PostId;
        }
    }
}
