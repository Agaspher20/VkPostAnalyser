using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using VkPostAnalyser.Model;
using VkPostAnalyser.Services.Authentication;

namespace VkPostAnalyser.Services
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext() : base("DefaultConnection")
        {
        }

        public DbSet<PostInfo> PostInfos { get; set; }

        public DbSet<UserReport> UserReports { get; set; }
    }
}
