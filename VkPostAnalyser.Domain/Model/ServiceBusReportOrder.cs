namespace VkPostAnalyser.Domain.Model
{
    public class ServiceBusReportOrder
    {
        public int UserId { get; set; }

        public ApplicationUser Author { get; set; }
    }
}
