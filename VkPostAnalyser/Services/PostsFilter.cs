using System;
using System.Linq;
using System.Collections.Generic;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    internal static class PostsFilter
    {
        public static IList<PostInfo> FilterPosts(this IList<PostInfo> posts)
        {
            if (posts.Count == 0)
            {
                return posts;
            }
            var equalityComparer = new DeltaIntEqualityComparer(10);
            var maxLikesCount = posts.Max(p => p.LikesCount);
            var mostPopularPost = posts.First(p => p.LikesCount == maxLikesCount);
            return posts.GroupBy(p => p.SignsCount, (sc, pg) => BuildFromGroup(sc, mostPopularPost.PostId, pg), equalityComparer).ToList();
        }

        private static PostInfo BuildFromGroup(int signsCount, long mostPopularId, IEnumerable<PostInfo> postsGroup)
        {
            var postsList = postsGroup.OrderBy(p => p.LikesCount).ToList();
            int maxLikesCount = postsList.Max(p => p.LikesCount), likesCount;
            long postId = postsList.First(p => p.LikesCount == maxLikesCount).PostId;
            if (postId == mostPopularId)
            {
                likesCount = maxLikesCount;
            }
            else
            {
                int halfCount = postsList.Count / 2, remainder = postsList.Count % 2;
                int medianLikesCount = remainder == 0
                    ? (postsList[halfCount - 1].LikesCount + postsList[halfCount].LikesCount) / 2
                    : postsList[halfCount].LikesCount;
                likesCount = medianLikesCount;
            }
            
            return new PostInfo
            {
                LikesCount = likesCount,
                SignsCount = signsCount,
                PostId = postId
            };
        }
    }

    internal class DeltaIntEqualityComparer : IEqualityComparer<int>
    {
        private readonly int _delta;

        public DeltaIntEqualityComparer(int delta)
        {
            _delta = Math.Abs(delta);
        }
        public bool Equals(int x, int y)
        {
            return RoundValue(x) == RoundValue(y);
        }

        public int GetHashCode(int obj)
        {
            return RoundValue(obj).GetHashCode();
        }

        private int RoundValue(int obj)
        {
            int remainder = obj % _delta;
            return obj - remainder + (remainder >= _delta/2 ? _delta : 0);
        }
    }
}
