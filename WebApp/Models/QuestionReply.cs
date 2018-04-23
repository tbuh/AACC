using Newtonsoft.Json;
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
        public byte[] ReportImage { get; set; }
        public string SubQuestionReply { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<QuestionReply> SubQuestionList { get; set; }

        [JsonIgnore]
        [NotMapped]
        public string Title
        {
            get
            {
                return Question.Title;
            }
        }

        public QuestionReply()
        {

        }


        public QuestionReply(Question q)
        {
            Question = q;
            QuestionId = q.QuestionId;
            Load(q.Questions?.Count != 0 ? q.Questions : new List<Question>() { q });
        }

        private void Load(IEnumerable<Question> question)
        {
            SubQuestionList = (from q2 in question
                               select new QuestionReply
                               {
                                   Question = q2,
                                   QuestionId = q2.QuestionId
                               }).ToList();
        }
    }
}
