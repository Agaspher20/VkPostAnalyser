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

        public void SavePosts(IEnumerable<PostInfo> postInfos)
        {
            foreach (var post in postInfos)
            {
                _dataContext.PostInfos.Add(post);
            }
            _dataContext.SaveChanges();
        }
    }
}
