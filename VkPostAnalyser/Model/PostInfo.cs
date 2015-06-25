﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VkPostAnalyser.Model
{
    public class PostInfo
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long PostId { get; set; }
        [Required]
        public int SignsCount { get; set; }
        [Required]
        public int LikesCount { get; set; }

        public int OwnerId { get; set; }

        [NotMapped]
        public string Link { get; set; }
    }
}
