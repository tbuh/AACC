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
        public byte[] ReportImage { get; set; }
        [NotMapped]
        public string SubQuestionReply { get; set; }
        [NotMapped]
        public int AccreditationStandartId { get; set; }
        [NotMapped]
        public int? QuestionParentId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<QuestionReply> SubQuestionList { get; set; }

        [JsonIgnore]
        [NotMapped]
        public string Title
        {
            get; set;
        }

        public QuestionReply()
        {

        }


        public QuestionReply(Question q)
        {
            SubQuestionList = new List<QuestionReply>();
            AccreditationStandartId = q.AccreditationStandartId.Value;
            Question = q;
            QuestionId = q.QuestionId;
            Title = q.Title;
            Load(q.Questions?.Count != 0 ? q.Questions.ToList() : new List<Question>() { q });
        }

        private void Load(List<Question> question)
        {
            for (int i = 0; i < question.Count(); i++)
            {
                var q2 = question[i];
                SubQuestionList.Add(new QuestionReply
                {
                    QuestionParentId = Question.QuestionId,
                    Title = q2.Title,
                    Question = q2,
                    QuestionId = q2.QuestionId,
                    AccreditationStandartId = AccreditationStandartId,
                    QuestionNumber = (i + 1).ToString()
                });
            }
        }
    }
}
