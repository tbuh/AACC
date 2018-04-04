using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class QuestionReply
    {
        public int QuestionReplyId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public bool Response { get; set; }
        public string Notes { get; set; }
        [Required]
        public int ReportId { get; set; }


    }
}
