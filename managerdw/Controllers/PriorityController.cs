using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using managerdw.Models;

namespace managerdw.Controllers
{
    public class PriorityController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public RedirectToRouteResult Create(Priority model)
        {
            if (ModelState.IsValid)
            {
                Priority pr = new Priority
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                };

                dbContext.Priorities.Add(pr);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult All()
        {
            var list = dbContext.Priorities.ToList();
            return View(list);
        }

        [Authorize]
        [HttpGet]
        public RedirectToRouteResult Delete(Guid Id)
        {
            var select = dbContext.Priorities.FirstOrDefault(a => a.Id == Id);

            if (select != null)
            {
                dbContext.Priorities.Remove(select);
                dbContext.SaveChanges();
            }

            return RedirectToAction("All");
        }
    }
}