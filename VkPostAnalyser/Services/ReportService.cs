using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkPostAnalyser.Model;
using VkPostAnalyser.Services.Authentication;

namespace VkPostAnalyser.Services
{
    public interface IReportService
    {
        ReportsViewModel NextReportsPage(int? authorId, DateTime? date, int pageSize);

        ReportsViewModel RetrieveNewReports(int? authorId, DateTime date);

        Task<UserReport> CreateReportAsync(int userId, ApplicationUser user);
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

        public ReportsViewModel NextReportsPage(int? authorId, DateTime? date, int pageSize)
        {
            IQueryable<UserReport> reportsQuery = _dataContext.UserReports.Include("PostInfos");
            if (authorId.HasValue)
            {
                reportsQuery = reportsQuery.Where(r => r.AuthorId == authorId.Value);
            }
            if (date.HasValue)
            {
                reportsQuery = reportsQuery.Where(r => r.CreationDate < date.Value);
            }
            reportsQuery = reportsQuery.OrderByDescending(r => r.CreationDate).Take(pageSize);

            return BuildReportsModel(reportsQuery.ToList(), pageSize);
        }

        public ReportsViewModel RetrieveNewReports(int? authorId, DateTime date)
        {
            IQueryable<UserReport> reportsQuery = _dataContext.UserReports.Include("PostInfos");
            if (authorId.HasValue)
            {
                reportsQuery = reportsQuery.Where(r => r.AuthorId == authorId.Value);
            }
            reportsQuery = reportsQuery
                .Where(r => r.CreationDate > date)
                .OrderByDescending(r => r.CreationDate);

            return BuildReportsModel(reportsQuery.ToList());
        }

        public async Task<UserReport> CreateReportAsync(int userId, ApplicationUser author)
        {
            DateTime currentDate = DateTime.Now;
            IList<PostInfo> allPosts = await _socialApiProvider.RetrievePostInfosAsync(userId, author);
            var userReport = new UserReport
            {
                AuthorId = author == null ? null : (int?)author.Id,
                CreationDate = currentDate,
                UserId = userId,
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
