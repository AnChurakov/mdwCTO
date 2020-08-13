using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace managerdw.Models
{
    public class Priority
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Название приоритета")]
        public string Name { get; set; }

        public ICollection<TaskProject> Tasks { get; set; }
    }
}