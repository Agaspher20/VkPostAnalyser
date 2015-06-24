using System.Collections.Generic;
using System.Linq;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public interface IRepository
    {
        IQueryable<PostInfo> PostInfos { get; }

        IQueryable<UserReport> UserReports { get; }

        void SaveReport(UserReport report);
    }
}
