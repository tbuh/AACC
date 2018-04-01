using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Assessor> Assessors { get; set; }
    }
}
