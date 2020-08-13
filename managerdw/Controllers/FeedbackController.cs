using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using managerdw.Models;

namespace managerdw.Controllers
{
    public class FeedbackController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index(Guid Id)
        {           
            var project = context.Projects.FirstOrDefault(a => a.Id == Id);

            if (project.Feedbacks.Select(t => t.Id == Id).Count() != 0)
            {
                return RedirectToAction("HappyEnd", new { Id });
            }
            ViewBag.ProjectId = project.Id;
            ViewBag.ProjectName = project.Name;

            return View();
            
        }

        [Authorize(Roles = "Руководитель")]
        public ActionResult FeedbackProject(Guid Id)
        {
            var answers = context.Feedbacks.FirstOrDefault(a => a.Projects.Id == Id);

            return View(answers);
        }

        [Authorize]
        public ActionResult HappyEnd(Guid Id)
        {
            var project = context.Projects.FirstOrDefault(t => t.Id == Id);

            ViewBag.Name = project.Name;

            return View();
        }
        
        [Authorize]
        [HttpPost]
        public bool AddNewFeedback(string comment, string price, string dld, string edit, string ProjectId)
        {
            if (price != string.Empty && dld != string.Empty && edit != string.Empty)
            {
                var project = context.Projects.FirstOrDefault(t => t.Id == new Guid(ProjectId));

                Feedback fd = new Feedback
                {
                    Id = Guid.NewGuid(),
                    Comment = comment,
                    Deadline = dld,
                    Edit = edit,
                    Price = price,
                    Projects = project
                };

                context.Feedbacks.Add(fd);
                context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}