using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VkPostAnalyser.Model
{
    public class UserReport
    {
        [Key]
        public long Id { get; set; }

        public string AuthorId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Index]
        [Required]
        public DateTime CreationDate { get; set; }

        public ICollection<PostInfo> PostInfos { get; set; }
    }
}
