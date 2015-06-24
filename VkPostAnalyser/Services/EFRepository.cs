using System.Collections.Generic;
using System.Linq;
using VkPostAnalyser.Model;

namespace VkPostAnalyser.Services
{
    public class EfRepository : IRepository
    {
        private readonly DataContext _dataContext = new DataContext();

        public IQueryable<PostInfo> PostInfos
        {
            get
            {
                return _dataContext.PostInfos;
            }
        }

        public IQueryable<UserReport> UserReports
        {
            get
            {
                return _dataContext.UserReports.Include("PostInfos");
            }
        }

        public void SaveReport(UserReport report)
        {
            _dataContext.UserReports.Add(report);
            _dataContext.SaveChanges();
        }
    }
}
