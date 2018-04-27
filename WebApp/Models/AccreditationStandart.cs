using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class AccreditationStandart
    {
        [Key]
        public int AccreditationStandartId { get; set; }
        public ICollection<Question> Questions { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        [Display(Name = "Standard Type")]
        public StandartType StandartType { get; set; }

    }
}
