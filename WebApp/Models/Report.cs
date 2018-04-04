using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        [Required]
        public int AgedCareCenterId { get; set; }
        [Required]
        public int AssessorId { get; set; }
        public DateTime ReportDate { get; set; }
        public double CompletionStatus { get; set; }
        public string Notes { get; set; }
        public ICollection<QuestionReply> QuestionReply { get; set; }

    }
}
