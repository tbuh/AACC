﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        [NotMapped]
        public int QuestionNumber { get; set; }
        public int? AccreditationStandartId { get; set; }
        public int? ParentId { get; set; }
        [JsonIgnore]
        public Question Parent { get; set; }

        [ForeignKey("ParentId")]
        [JsonIgnore]
        public ICollection<Question> Questions { get; set; }
        [ForeignKey("QuestionId")]
        [JsonIgnore]
        public ICollection<QuestionReply> QuestionReplies { get; set; }
    }

    public class SubQuestion
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public bool IsMet { get; set; }
        [NotMapped]
        [JsonIgnore]
        public int Number { get; set; }
    }
}
