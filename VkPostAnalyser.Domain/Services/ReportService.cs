using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkPostAnalyser.Domain.Model;

namespace VkPostAnalyser.Domain.Services
{
    public interface IReportService
    {
        ReportsViewModel NextReportsPage(int? authorId, DateTime? date, int pageSize);

        ReportsViewModel RetrieveNewReports(int? authorId, DateTime date, IList<int> skipIds);

        Task CreateReportAsync(int userId, ApplicationUser user);
    }

    public class ReportService : IReportService
    {
        private readonly DataContext _dataContext;
        private readonly ISocialApiProvider _socialApiProvider;
        private readonly IReportsQueueConnector _reportsQueueConnector;

        public ReportService(DataContext dataContext, ISocialApiProvider socialApiProvider, IReportsQueueConnector reportsQueueConnector)
        {
            _dataContext = dataContext;
            _socialApiProvider = socialApiProvider;
            _reportsQueueConnector = reportsQueueConnector;
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

        public ReportsViewModel RetrieveNewReports(int? authorId, DateTime date, IList<int> skipIds)
        {
            IQueryable<UserReport> reportsQuery = _dataContext.UserReports.Include("PostInfos");
            if (authorId.HasValue)
            {
                reportsQuery = reportsQuery.Where(r => r.AuthorId == authorId.Value);
            }
            if (skipIds != null && skipIds.Any())
            {
                reportsQuery = reportsQuery.Where(r => !skipIds.Contains(r.Id));
            }
            reportsQuery = reportsQuery
                .Where(r => r.CreationDate > date)
                .OrderByDescending(r => r.CreationDate);

            return BuildReportsModel(reportsQuery.ToList());
        }

        public async Task CreateReportAsync(int userId, ApplicationUser author)
        {
            // Create a message from the order
            var message = new BrokeredMessage(new ServiceBusReportOrder
            {
                UserId = userId,
                Author = author
            });

            // Submit the order
            await _reportsQueueConnector.ReportsQueueClient.SendAsync(message);
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
