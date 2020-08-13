using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace managerdw.Models
{
    public class TaskProject
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Название задачи")]
        public string Name { get; set; }

        public DateTime? Create { get; set; }

        public virtual Priority Prioritys { get; set; }

        public virtual Status Status { get; set; }

        public virtual Project Project { get; set; }

        public virtual Stage Stages { get; set; }
    }
}