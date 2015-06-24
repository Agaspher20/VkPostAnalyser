using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Web.Http;
using VkPostAnalyser.Model;
using VkPostAnalyser.Services;

namespace VkPostAnalyser.Controllers
{
    public class ReportsController : ApiController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public ReportsViewModel Get(DateTime? lastDate = null, int pageSize = 10, bool mineOnly = false)
        {
            return _reportService.RetrieveReports(null, lastDate, pageSize);
        }

        public async Task<IHttpActionResult> Post(ReportOrder order)
        {
            UserReport report;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                report = await _reportService.CreateReportAsync(order.UserId);
            }
            catch (DbEntityValidationException exc)
            {
                return InternalServerError();
            }
            string uri = Url.Link("DefaultApi", new { id = report.AuthorId });
            return Created<UserReport>(uri, report);
        }
    }
}
