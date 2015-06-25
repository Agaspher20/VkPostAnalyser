using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public interface IReportService
    {
        ReportsViewModel RetrieveReports(string authorId, DateTime? lastDate, int pageSize);

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

        public ReportsViewModel RetrieveReports(string authorId, DateTime? lastDate, int pageSize)
        {
            var reportsQuery = _repository.UserReports;
            if (authorId != null)
            {
                reportsQuery = reportsQuery.Where(r => r.AuthorId == authorId);
            }
            if (lastDate.HasValue)
            {
                reportsQuery = reportsQuery.Where(r => r.CreationDate < lastDate.Value);
            }

            var model = new ReportsViewModel
            {
                Reports = reportsQuery.OrderByDescending(r => r.CreationDate).Take(pageSize).ToList()
            };
            model.LastDate = model.Reports.Any() ? (DateTime?)model.Reports.Max(r => r.CreationDate) : null;
            foreach (var report in model.Reports)
            {
                InitUserReport(report);
            }
            return model;
        }

        public async Task<UserReport> CreateReportAsync(string userId, string authorId)
        {
            DateTime currentDate = DateTime.Now;
            IEnumerable<PostInfo> posts = await _socialApiProvider.RetrievePostInfosAsync(userId);
            var userReport = new UserReport
            {
                AuthorId = authorId,
                CreationDate = currentDate,
                UserId = userId,
                PostInfos = posts.ToList()
            };
            _repository.SaveReport(userReport);
            InitUserReport(userReport);
            return userReport;
        }

        private void InitUserReport(UserReport report)
        {
            if (report.MostPopular != null)
            {
                report.MostPopular.Link = _socialApiProvider.BuildPostUrl(report.MostPopular);
            }
        }
    }
}
