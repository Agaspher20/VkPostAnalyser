using System.ComponentModel.DataAnnotations;
using VkPostAnalyser.Resources;

namespace VkPostAnalyser.Model
{
    public class ReportOrder
    {
        [Required(ErrorMessageResourceName = "UserIdRequired", ErrorMessageResourceType=typeof(LocalizationStrings))]
        public string UserId { get; set; }
    }
}