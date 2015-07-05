using System.ComponentModel.DataAnnotations;

namespace VkPostAnalyser.Domain.Model
{
    public class ReportOrder
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? UserId { get; set; }
    }

    public class ServiceBusReportOrder : ReportOrder
    {
        public ApplicationUser Author { get; set; }
    }
}
