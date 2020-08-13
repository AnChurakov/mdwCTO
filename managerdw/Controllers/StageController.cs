using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using managerdw.Models;

namespace managerdw.Controllers
{
    public class StageController : Controller
    {
       ApplicationDbContext dbContext = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public RedirectToRouteResult Create(Stage model)
        {
            if (ModelState.IsValid)
            {
                Stage stage = new Stage
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                  
                };

                dbContext.Stages.Add(stage);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult All()
        {
            return View(dbContext.Stages.ToList());
        }

        [Authorize(Roles = "Руководитель")]
        public Stage Get(string name)
        {
            var select = dbContext.Stages.FirstOrDefault(a => a.Name == name);
            return select;
        }


        [Authorize]
        [HttpGet]
        public RedirectToRouteResult Delete(Guid Id)
        {
            var select = dbContext.Stages.FirstOrDefault(a => a.Id == Id);

            if (select != null)
            {
                dbContext.Stages.Remove(select);

                dbContext.SaveChanges();
            }

            return RedirectToAction("All");
        }

    }
}