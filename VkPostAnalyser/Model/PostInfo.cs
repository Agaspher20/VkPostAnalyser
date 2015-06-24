using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VkPostAnalyser.Model
{
    public class PostInfo
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string PostId { get; set; }
        public string AuthorId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int SignsCount { get; set; }
        [Required]
        public int LikesCount { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
