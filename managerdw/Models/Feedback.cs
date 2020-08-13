using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace managerdw.Models
{
    public class Feedback
    {
        public Guid Id { get; set; }

        //Плата за разработку
        public string Price { get; set; }

        //Соблюедены ли все сроки сдачи
        public string Deadline { get; set; }

        //Многи ли правок было внесено
        public string Edit { get; set; }

        //Доплнительный комментарий
        public string Comment { get; set; }

        public virtual Project Projects { get; set; }
    }
}