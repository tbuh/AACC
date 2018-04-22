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
        public List<SubQuestion> SubQuestionList { get; set; }

        public void Update()
        {
            if (SubQuestionList != null)
                SubQuestionReply = JsonConvert.SerializeObject(SubQuestionList);
        }

        public void Load()
        {
            if (SubQuestionList != null) return;

            if (string.IsNullOrEmpty(SubQuestionReply))
            {
                if (!string.IsNullOrEmpty(Question.SubQuestions))
                    SubQuestionList = JsonConvert.DeserializeObject<List<SubQuestion>>(Question.SubQuestions);
                else
                    SubQuestionList = new List<SubQuestion>();
            }
            else
            {
                SubQuestionList = JsonConvert.DeserializeObject<List<SubQuestion>>(SubQuestionReply);
            }
        }
    }
}
