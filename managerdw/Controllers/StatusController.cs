using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using managerdw.Models;

namespace managerdw.Controllers
{
    public class StatusController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public RedirectToRouteResult Create(Status model)
        {
            if (ModelState.IsValid)
            {
                Status status = new Status
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                };

                dbContext.Status.Add(status);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult All()
        {
            return View(dbContext.Status.ToList());
        }

        [Authorize]
        [HttpGet]
        public RedirectToRouteResult Delete(Guid Id)
        {
            var select = dbContext.Status.FirstOrDefault(a => a.Id == Id);

            if (select != null)
            {
                dbContext.Status.Remove(select);

                dbContext.SaveChanges();
            }

            return RedirectToAction("All");
        }

        [Authorize(Roles = "Руководитель")]
        public Status Get(string name)
        {
            return dbContext.Status.FirstOrDefault(a => a.Name == name);
        }
    }
}