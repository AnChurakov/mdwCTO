using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using managerdw.Models;
using MvcSiteMapProvider;

namespace managerdw.Controllers
{
    public class TaskController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        [Authorize]
        public ActionResult AllTaskProject(Guid Id)
        {
            ViewBag.NameProject = GetProject(Id).Name;
            ViewBag.ProjectId = Id;

            ViewBag.Status = dbContext.Status.ToList();
            ViewBag.Stages = dbContext.Stages.ToList();
            ViewBag.Prior = dbContext.Priorities.ToList();

            var select = dbContext.Tasks.Where(a => a.Project.Id == Id).OrderByDescending(t => t.Create).ToList();
            SiteMaps.Current.CurrentNode.ParentNode.Title = dbContext.Projects.FirstOrDefault(t => t.Id == Id).Name;

            return View(select);
        }

        [Authorize]
        public ActionResult Add(Guid Id)
        {
            ViewBag.Prior = dbContext.Priorities.ToList();
            ViewBag.Status = dbContext.Status.ToList();
            ViewBag.Stages = dbContext.Stages.ToList();

            ViewBag.ProjectId = Id;

            SiteMaps.Current.CurrentNode.ParentNode.Title = dbContext.Projects.FirstOrDefault(t => t.Id == Id).Name;

            return View();
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult Create(string Name, Guid PriorId, Guid StatusId, Guid ProjectId, Guid StageId)
        {
            if (Name != null && PriorId != null && StatusId != null)
            {
                TaskProject task = new TaskProject {

                    Id = Guid.NewGuid(),
                    Name = Name,
                    Create = DateTime.Now,
                    Prioritys = dbContext.Priorities.FirstOrDefault(a => a.Id == PriorId),
                    Status = dbContext.Status.FirstOrDefault(a => a.Id == StatusId),
                    Project = GetProject(ProjectId),
                    Stages = GetStage(StageId)

                };

                dbContext.Tasks.Add(task);
                dbContext.SaveChanges();

            }
            
            return RedirectToAction("Add", new { Id = ProjectId});
        }

        [Authorize]
        public Stage GetStage(Guid Id)
        {
            return dbContext.Stages.FirstOrDefault(a => a.Id == Id);
        }

        [Authorize]
        public Project GetProject(Guid Id)
        {
            return dbContext.Projects.FirstOrDefault(a => a.Id == Id);
        }

        [Authorize]
        [HttpPost]
        public bool UpdateStatusTask(Guid StatusId, Guid TaskId, string ProjectId)
        {
            var task = dbContext.Tasks.FirstOrDefault(a => a.Id == TaskId);
            var status = dbContext.Status.FirstOrDefault(t => t.Id == StatusId);

            if (task != null && status != null)
            {
                task.Status = status;
                dbContext.SaveChanges();

                return true;
            }

            return false;
        }

        [Authorize]
        [HttpPost]
        public bool UpdatePriorTask(Guid PriorId, Guid TaskId, string ProjectId)
        {
            var task = dbContext.Tasks.FirstOrDefault(a => a.Id == TaskId);
            var prior = dbContext.Priorities.FirstOrDefault(t => t.Id == PriorId);

            if (task != null && prior != null)
            {
                task.Prioritys = prior;
                dbContext.SaveChanges();

                return true;
            }

            return false;
        }

        [Authorize]
        [HttpPost]
        public bool UpdateStageTask(Guid StageId, Guid TaskId, string ProjectId)
        {
            var task = dbContext.Tasks.FirstOrDefault(a => a.Id == TaskId);
            var stage = dbContext.Stages.FirstOrDefault(t => t.Id == StageId);

            if (task != null && stage != null)
            {
                task.Stages = stage;

                dbContext.SaveChanges();

                return true;
            }

            return false;
        }

        [Authorize]
        [HttpGet]
        public RedirectToRouteResult Delete(Guid Id, Guid ProjectId)
        {
            var select = dbContext.Tasks.FirstOrDefault(a => a.Id == Id);

            if (select != null)
            {
                dbContext.Tasks.Remove(select);
                dbContext.SaveChanges();
            }

            return RedirectToAction("AllTaskProject", new { id = ProjectId});
        }
    }
}