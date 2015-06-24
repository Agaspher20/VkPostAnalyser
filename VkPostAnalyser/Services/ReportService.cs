using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public interface IReportService
    {
        IEnumerable<UserReport> RetrieveReports(string authorId = null);

        Task<UserReport> CreateReportAsync(string userId, string authorId = null);
    }

    public class ReportService : IReportService
    {
        private readonly IRepository _repository;
        private readonly ISocialApiProvider _socialApiProvider;

        public ReportService(IRepository repository, ISocialApiProvider socialApiProvider)
        {
            _repository = repository;
            _socialApiProvider = socialApiProvider;
        }

        public IEnumerable<UserReport> RetrieveReports(string authorId)
        {
            var postInfosQuery = _repository.PostInfos;
            if (authorId != null)
            {
                postInfosQuery = postInfosQuery
                    .Where(pi => pi.AuthorId == authorId);
            }
            return postInfosQuery.GroupBy(pi => new { pi.CreationDate, pi.UserId, pi.AuthorId }, (k, pig) => new UserReport
            {
                PostInfos = pig,
                CreationDate = k.CreationDate,
                UserId = k.UserId,
                AuthorId = k.AuthorId
            }).ToList();
        }

        public async Task<UserReport> CreateReportAsync(string userId, string authorId)
        {
            DateTime currentDate = DateTime.Now;
            IEnumerable<PostInfo> posts = await _socialApiProvider.RetrievePostInfosAsync(userId);
            foreach(var post in posts) {
                post.CreationDate = currentDate;
                post.AuthorId = authorId;
            }
            _repository.SavePosts(posts);
            return new UserReport
            {
                PostInfos = posts,
                UserId = userId,
                AuthorId = authorId
            };
        }
    }
}
