using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkPostAnalyser.Domain.Model;

namespace VkPostAnalyser.Domain.Services
{
    public interface IReportBuilder
    {
        Task BuildReportAsync(ServiceBusReportOrder reportOrder);
    }

    public class ReportBuilder : IReportBuilder
    {
        private readonly DataContext _dataContext;
        private readonly ISocialApiProvider _socialApiProvider;

        public ReportBuilder(DataContext dataContext, ISocialApiProvider socialApiProvider)
        {
            _dataContext = dataContext;
            _socialApiProvider = socialApiProvider;
        }

        public async Task BuildReportAsync(ServiceBusReportOrder reportOrder)
        {
            DateTime currentDate = DateTime.Now;
            IList<PostInfo> allPosts = await _socialApiProvider.RetrievePostInfosAsync(reportOrder.UserId, reportOrder.Author);
            var userReport = new UserReport
            {
                AuthorId = reportOrder.Author == null ? null : (int?)reportOrder.Author.Id,
                CreationDate = currentDate,
                UserId = reportOrder.UserId,
                PostInfos = allPosts
            };
            _dataContext.UserReports.Add(userReport);
            _dataContext.SaveChanges();
        }
    }
}
