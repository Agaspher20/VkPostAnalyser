using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Web.Http;
using VkPostAnalyser.Model;
using VkPostAnalyser.Services;
using VkPostAnalyser.Services.Authentication;
using VKSharp.Helpers.Exceptions;

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
        public ReportsViewModel NextPage(DateTime? date = null, int pageSize = 5, bool mineOnly = false)
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
            return _reportService.NextReportsPage(userId, date, pageSize);
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
                report = await _reportService.CreateReportAsync(order.UserAlias, user);
            }
            catch (DbEntityValidationException exc)
            {
                return InternalServerError(exc);
            }
            catch (VKException exc)
            {
                return InternalServerError(exc);
            }
            string uri = Url.Link("DefaultApi", new { id = report.AuthorId });
            return Created<UserReport>(uri, report);
        }
    }
}
