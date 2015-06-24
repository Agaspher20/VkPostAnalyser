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

        [HttpGet]
        public IEnumerable<UserReport> Get(string id)
        {
            return _reportService.RetrieveReports();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]string id)
        {
            UserReport report;
            try
            {
                report = await _reportService.CreateReportAsync(id);
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
