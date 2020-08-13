using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace managerdw.Models
{
    public class Status
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Название статуса")]
        public string Name { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<TaskProject> Task { get; set; }

    }
}