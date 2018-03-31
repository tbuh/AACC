using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class AccreditationStandartL2
    {
        [Key]
        public int AS_L2Id { get; set; }
        public int AS_L1Id { get; set; }
        public string Notes { get; set; }
        public int QuestionsL2Id { get; set; }
        public string Description { get; set; }
        public string Standart2Name { get; set; }
        public double CompletionStatus { get; set; }

    }
}
