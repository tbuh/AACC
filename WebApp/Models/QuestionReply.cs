using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class QuestionReply
    {
        public int QuestionReplyId { get; set; }
        public bool Response { get; set; }
        public string Notes { get; set; }
        public Question Question { get; set; }
        
    }
}
