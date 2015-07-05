using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VkPostAnalyser.Resources;
using VkPostAnalyser.Services.Authentication;

namespace VkPostAnalyser.Model
{
    public class ReportOrder
    {
        [Required(ErrorMessageResourceName = "UserIdRequired", ErrorMessageResourceType=typeof(LocalizationStrings))]
        [Range(1, int.MaxValue)]
        public int? UserId { get; set; }
    }

    public class ServiceBusReportOrder : ReportOrder
    {
        public ApplicationUser Author { get; set; }
    }
}
