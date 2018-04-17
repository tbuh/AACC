using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        [Required]
        public int AgedCareCenterId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public AgedCareCenter AgedCareCenter { get; set; }
        [Required]
        public int AssessorId { get; set; }
        public DateTime ReportDate { get; set; }
        public double CompletionStatus { get; set; }
        public string Notes { get; set; }
        public ICollection<QuestionReply> QuestionReply { get; set; }
        [NotMapped]
        public bool IsDeleted { get; set; }
        [NotMapped]
        public bool IsChanged { get; set; }
        public byte[] ReportImage { get; set; }
        [NotMapped]
        public bool IsNew { get; set; }
    }
}
