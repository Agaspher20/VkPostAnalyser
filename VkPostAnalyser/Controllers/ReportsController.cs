using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Web.Http;
using VkPostAnalyser.Model;
using VkPostAnalyser.Services;
using VkPostAnalyser.Services.Authentication;
using VkPostAnalyser.Services.VkApi;
using VKSharp.Helpers.Exceptions;
using System.Net;

namespace VkPostAnalyser.Controllers
{
    public class ReportsController : ApiController
    {
        private readonly IReportService _reportService;
        private readonly UserManager<ApplicationUser, int> _userManager;

        public ReportsController(IReportService reportService, UserManager<ApplicationUser, int> userManager)
        {
            _reportService = reportService;
            _userManager = userManager;
        }

        [HttpGet]
        public IHttpActionResult NextPage(DateTime? date = null, int pageSize = 5, bool mineOnly = false)
        {
            int? userId;

            if (mineOnly && User.Identity.IsAuthenticated)
            {
                userId = int.Parse(User.Identity.GetUserId());
            }
            else
            {
                userId = null;
            }
            var reportsModel = _reportService.NextReportsPage(userId, date, pageSize);
            var status = !reportsModel.Reports.Any() ? HttpStatusCode.NoContent : HttpStatusCode.OK;
            return Content<ReportsViewModel>(status, reportsModel);
        }

        [HttpGet]
        public IHttpActionResult NewReports(DateTime date, bool mineOnly = false)
        {
            var reportsModel = _reportService.RetrieveNewReports(null, date);
            var status = !reportsModel.Reports.Any() ? HttpStatusCode.NoContent : HttpStatusCode.OK;
            return Content<ReportsViewModel>(status, reportsModel);
        }

        [HttpPost]
        [VkExceptionFilter]
        public async Task<IHttpActionResult> Post(ReportOrder order)
        {
            UserReport report;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user;

            if (User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(User.Identity.GetUserId());
                user = await _userManager.FindByIdAsync(userId);
            }
            else
            {
                user = null;
            }
            report = await _reportService.CreateReportAsync(order.UserId.Value, user);
            string uri = Url.Link("DefaultApi", new { id = report.AuthorId });
            return Created<UserReport>(uri, report);
        }
    }
}
