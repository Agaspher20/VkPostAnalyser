using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace VkPostAnalyser.Model
{
    public class UserReport
    {
        private readonly Lazy<PostInfo> _mostPopular;

        public UserReport()
        {
            _mostPopular = new Lazy<PostInfo>(() =>
            {
                if (PostInfos == null || !PostInfos.Any())
                {
                    return null;
                }
                return PostInfos.OrderByDescending(pi => pi.LikesCount).First();
            });
        }

        [Key]
        public long Id { get; set; }

        public string AuthorId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Index]
        [Required]
        public DateTime CreationDate { get; set; }

        public ICollection<PostInfo> PostInfos { get; set; }

        [NotMapped]
        public PostInfo MostPopular
        {
            get
            {
                return _mostPopular.Value;
            }
        }
    }
}
