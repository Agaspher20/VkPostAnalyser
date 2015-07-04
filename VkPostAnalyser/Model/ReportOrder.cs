using System.ComponentModel.DataAnnotations;
using VkPostAnalyser.Resources;

namespace VkPostAnalyser.Model
{
    public class ReportOrder
    {
        [Required(ErrorMessageResourceName = "UserIdRequired", ErrorMessageResourceType=typeof(LocalizationStrings))]
        [Range(1, int.MaxValue)]
        public int? UserId { get; set; }
    }
}