using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class QuestionReply
    {
        public int QuestionReplyId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public bool Response { get; set; }
        public string Notes { get; set; }
        [Required]
        public int ReportId { get; set; }
        [NotMapped]
        public string QuestionNumber { get; set; }
        [NotMapped]
        [Newtonsoft.Json.JsonIgnore]
        public int QuestionNumberOrderBy { get; set; }
        [NotMapped]
        public int AccreditationStandartId { get; set; }
        [NotMapped]
        public int QuestionText { get; set; }
    }
}
