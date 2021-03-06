﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Assessor
    {
        public int AssessorId { get; set; }
        public bool IsAdmin { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [JsonIgnore]
        public string Login { get; set; }
        [Required]
        [JsonIgnore]
        public string Password { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        [JsonIgnore]
        public ICollection<Report> Reports { get; set; }
    }
}
