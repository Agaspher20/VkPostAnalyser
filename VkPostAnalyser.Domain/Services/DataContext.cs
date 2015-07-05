using System.Data.Entity;
using VkPostAnalyser.Domain.Model;

namespace VkPostAnalyser.Domain.Services
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") { }

        public DbSet<PostInfo> PostInfos { get; set; }

        public DbSet<UserReport> UserReports { get; set; }
    }
}
