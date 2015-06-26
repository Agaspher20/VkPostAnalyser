using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public interface IReportService
    {
        ReportsViewModel RetrieveReports(string authorId, DateTime? firstDate, DateTime? lastDate, int pageSize);

        Task<UserReport> CreateReportAsync(string userAlias, string authorId = null);
    }

    public class ReportService : IReportService
    {
        private readonly DataContext _dataContext;
        private readonly ISocialApiProvider _socialApiProvider;

        public ReportService(DataContext dataContext, ISocialApiProvider socialApiProvider)
        {
            _dataContext = dataContext;
            _socialApiProvider = socialApiProvider;
        }

        public ReportsViewModel RetrieveReports(string authorId, DateTime? firstDate, DateTime? lastDate, int pageSize)
        {
            IQueryable<UserReport> reportsQuery = _dataContext.UserReports.Include("PostInfos");
            if (authorId != null)
            {
                reportsQuery = reportsQuery.Where(r => r.AuthorId == authorId);
            }
            if (lastDate.HasValue)
            {
                reportsQuery = reportsQuery.Where(r => r.CreationDate < lastDate.Value);
            }
            if (firstDate.HasValue)
            {
                reportsQuery = reportsQuery.Where(r => r.CreationDate > firstDate.Value);
            }

            var reports = reportsQuery.OrderByDescending(r => r.CreationDate).Take(pageSize).ToList();
            var model = new ReportsViewModel
            {
                Reports = reports
            };
            model.FirstDate = reports.Any() ? (DateTime?)reports.Max(r => r.CreationDate) : null;
            model.LastDate = reports.Any() ? (DateTime?)reports.Min(r => r.CreationDate) : null;
            model.HasMore = reports.Count == pageSize;
            foreach (var report in reports)
            {
                InitUserReport(report);
            }
            return model;
        }

        public async Task<UserReport> CreateReportAsync(string userAlias, string authorId)
        {
            DateTime currentDate = DateTime.Now;
            IList<PostInfo> allPosts = await _socialApiProvider.RetrievePostInfosAsync(userAlias);
            int? ownerId = allPosts.Any() ? (int?)allPosts.First().OwnerId : null;
            var userReport = new UserReport
            {
                AuthorId = authorId,
                CreationDate = currentDate,
                UserId = ownerId,
                UserAlias = userAlias,
                PostInfos = allPosts.FilterPosts()
            };
            _dataContext.UserReports.Add(userReport);
            _dataContext.SaveChanges();
            InitUserReport(userReport);
            return userReport;
        }

        private void InitUserReport(UserReport report)
        {
            if (report.MostPopular != null)
            {
                report.MostPopular.Link = _socialApiProvider.BuildPostUrl(report, report.MostPopular);
            }
        }
    }
}
