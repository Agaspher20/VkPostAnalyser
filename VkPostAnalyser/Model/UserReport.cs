using System;
using System.Collections.Generic;
namespace VkPostAnalyser.Model
{
    public class UserReport
    {
        public string AuthorId { get; set; }
        public string UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public IEnumerable<PostInfo> PostInfos { get; set; }
    }
}
