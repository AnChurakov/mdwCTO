using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace managerdw.Models
{
    public class Project
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Название проекта")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Стоимость")]
        public string Price { get; set; }

        public DateTime? DateCreate { get; set; }

        //Дата завершения проекта по договору
        [Display(Name = "Дата завершения по договору")]
        public DateTime? DateComplete { get; set; }

        public virtual Stage Stages { get; set; }

        public virtual Status Status { get; set; }

        public virtual ICollection<TaskProject> Tasks { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
     

    }
}