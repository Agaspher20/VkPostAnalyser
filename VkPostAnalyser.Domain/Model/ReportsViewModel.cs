using System;
using System.Collections.Generic;

namespace VkPostAnalyser.Domain.Model
{
    public class ReportsViewModel
    {
        public DateTime? FirstDate { get; set; }
        public DateTime? LastDate { get; set; }
        public IEnumerable<UserReport> Reports { get; set; }
    }
}
