using System;
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

        [HttpGet]
        public ReportsViewModel NextPage(DateTime? date = null, int pageSize = 5, bool mineOnly = false)
        {
            return _reportService.NextReportsPage(null, date, pageSize);
        }

        [HttpGet]
        public ReportsViewModel NewReports(DateTime date, bool mineOnly = false)
        {
            return _reportService.RetrieveNewReports(null, date);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(ReportOrder order)
        {
            UserReport report;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                report = await _reportService.CreateReportAsync(order.UserAlias);
            }
            catch (DbEntityValidationException exc)
            {
                return InternalServerError(exc);
            }
            string uri = Url.Link("DefaultApi", new { id = report.AuthorId });
            return Created<UserReport>(uri, report);
        }
    }
}
