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

        public async Task<IList<PostInfo>> RetrievePostInfosAsync(string userAlias)
        {
            Task<List<PostInfo>> postsTask = Task.Run<List<PostInfo>>(() => new List<PostInfo>
            {
                new PostInfo
                {
                    LikesCount = 184,
                    PostId = 3,
                    SignsCount = 512
                },
                new PostInfo
                {
                    LikesCount = 384,
                    PostId = 7,
                    SignsCount = 1016
                },
                new PostInfo
                {
                    LikesCount = 2,
                    PostId = 7,
                    SignsCount = 102
                },
                new PostInfo
                {
                    LikesCount = 14,
                    PostId = 7,
                    SignsCount = 1015
                },
                new PostInfo
                {
                    LikesCount = 404,
                    PostId = 17,
                    SignsCount = 1224
                },
                new PostInfo
                {
                    LikesCount = 4,
                    PostId = 17,
                    SignsCount = 1220
                },
                new PostInfo
                {
                    LikesCount = 114,
                    PostId = 17,
                    SignsCount = 1218
                }
            });
            return await postsTask;

            //try
            //{
            //    var api = new VKApi();
            //    List<PostInfo> postInfos = null;
            //    int offset = 0;
            //    bool hasPosts;
            //    do
            //    {
            //        int pageCount = 0;
            //        EntityList<Post> posts = await api.Wall.Get(domain: userAlias, offset: offset, count: PageSize);
            //        if (postInfos == null)
            //        {
            //            postInfos = new List<PostInfo>(posts.Count);
            //        }
            //        foreach (var post in posts.Items)
            //        {
            //            ++pageCount;
            //            postInfos.Add(new PostInfo
            //            {
            //                LikesCount = post.Likes.Count,
            //                PostId = post.Id,
            //                SignsCount = post.Text.Length,
            //                OwnerId = post.OwnerId
            //            });
            //        }
            //        hasPosts = pageCount >= PageSize;
            //        offset += pageCount;
            //    }
            //    while (hasPosts);

            //    return postInfos;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public string BuildPostUrl(UserReport report, PostInfo post)
        {
            return BaseUrl + "/wall" + report.UserId.Value + "_" + post.PostId;
        }
    }
}
