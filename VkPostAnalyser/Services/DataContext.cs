using System.Data.Entity;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") { }

        public DbSet<PostInfo> PostInfos { get; set; }

        public DbSet<UserReport> UserReports { get; set; }
    }
}
