using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public interface IReportService
    {
        ReportsViewModel NextReportsPage(string authorId, DateTime? date, int pageSize);

        ReportsViewModel RetrieveNewReports(string authorId, DateTime date);

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

        public ReportsViewModel NextReportsPage(string authorId, DateTime? date, int pageSize)
        {
            IQueryable<UserReport> reportsQuery = _dataContext.UserReports.Include("PostInfos");
            if (authorId != null)
            {
                reportsQuery = reportsQuery.Where(r => r.AuthorId == authorId);
            }
            if (date.HasValue)
            {
                reportsQuery = reportsQuery.Where(r => r.CreationDate < date.Value);
            }
            reportsQuery = reportsQuery.OrderByDescending(r => r.CreationDate).Take(pageSize);

            return BuildReportsModel(reportsQuery.ToList(), pageSize);
        }

        public ReportsViewModel RetrieveNewReports(string authorId, DateTime date)
        {
            IQueryable<UserReport> reportsQuery = _dataContext.UserReports.Include("PostInfos");
            if (authorId != null)
            {
                reportsQuery = reportsQuery.Where(r => r.AuthorId == authorId);
            }
            reportsQuery = reportsQuery
                .Where(r => r.CreationDate > date)
                .OrderByDescending(r => r.CreationDate);

            return BuildReportsModel(reportsQuery.ToList());
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

        private ReportsViewModel BuildReportsModel(IList<UserReport> reports, int pageSize = int.MaxValue)
        {
            var model = new ReportsViewModel
            {
                Reports = reports
            };
            if (reports.Any())
            {
                model.FirstDate = (DateTime?)reports.Max(r => r.CreationDate);
                model.LastDate = (DateTime?)reports.Min(r => r.CreationDate);
            }
            model.HasMore = reports.Count == pageSize;
            foreach (var report in reports)
            {
                InitUserReport(report);
            }
            return model;
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
