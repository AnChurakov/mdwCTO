using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using managerdw.Models;
using managerdw.Controllers;
using System.Threading.Tasks;
using System.Data.Entity;
using MvcSiteMapProvider;

namespace managerdw.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        [Authorize]
        public ActionResult MyProject()
        {     
            List<Project> projects = new List<Project>();
            var user = dbContext.Users.Where(a => a.UserName == User.Identity.Name).FirstOrDefault();

            if(user != null)
            {
                Session["firstname"] = user.Firstname;
                Session["lastname"] = user.Lastname;
                Session["mail"] = user.Email;

                dbContext.Entry(user).Collection(c => c.Projects).Load();
                foreach (var project in user.Projects)
                {
                    projects.Add(project);
                }

                return View(projects);
            }

            return View();
        }

        [Authorize]
        public ActionResult Index()
        {
            var list = dbContext.Projects.OrderByDescending(a => a.DateCreate).ToList();
            return View(list);
        }

        [Authorize]
        public int GetCountProcent(Guid id, string stageName, string statusName)
        {
            var completed = dbContext.Tasks.Where(a => a.Stages.Name == stageName && a.Status.Name == statusName && a.Project.Id == id).Count();
            var total = dbContext.Tasks.Where(a => a.Stages.Name == stageName && a.Project.Id == id).Count();

            return Procent(total, completed);
        }

        [Authorize]
        public ActionResult Single(Guid Id)
        {
            var project = dbContext.Projects.FirstOrDefault(a => a.Id == Id);

            if (project != null)
            {
                if (project.Status.Name == "Выполнен" && (!User.IsInRole("Руководитель")))
                {
                    return RedirectToAction("Index", "Feedback", new { Id = Id });
                }
                else
                {
                    ViewBag.Tasks = dbContext.Tasks.Where(a => a.Project.Id == Id).OrderByDescending(t => t.Create).Take(5).ToList();

                    ViewBag.Design = GetCountProcent(Id, "Разработка дизайна", "Выполнен");
                    ViewBag.Develop = GetCountProcent(Id, "Разработка функционала", "Выполнен");
                    ViewBag.Analyze = GetCountProcent(Id, "Анализ требований", "Выполнен");
                    ViewBag.Testing = GetCountProcent(Id, "Тестирование", "Выполнен");

                    SiteMaps.Current.CurrentNode.Title = project.Name;
                }

                return View(project);

            }

            return View();

        }

        [Authorize]
        public int Procent(int total, int completed)
        {
            var result = 0;

            if (total != 0)
            {
                 result = (completed * 100) / total;
            }
            else if(total == 0)
            {
                result = 0;
            }
            else if(completed == total)
            {
                result = 100;
            }

            return result;
        }

        [Authorize(Roles = "Руководитель")]
        public ActionResult AddUser(Guid ProjectId)
        {
            ViewBag.ProjectId = ProjectId;
            ViewBag.Users = dbContext.Users.ToList();
            SiteMaps.Current.CurrentNode.ParentNode.Title = dbContext.Projects.FirstOrDefault(t => t.Id == ProjectId).Name;

            return View();
        }

        [Authorize(Roles = "Руководитель")]
        [HttpPost]
        public RedirectToRouteResult AddUserInProject(Guid ProjectId, string User)
        {
            var user = dbContext.Users.FirstOrDefault(a => a.Id == User);
            var project = dbContext.Projects.FirstOrDefault(a => a.Id == ProjectId);

            if (user != null && project != null)
            {
                project.Users = new List<ApplicationUser> { user };
                dbContext.SaveChanges();
            }

            return RedirectToAction("AddUser", new { ProjectId = ProjectId});
        }

        [Authorize(Roles = "Руководитель")]
        public ActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Руководитель")]
        public RedirectToRouteResult Create(string Name, string Description, string DateComplete, string Price)
        {
            var splitDate = DateComplete.Split('/');
            var newDate = String.Format("{0}.{1}.{2}", splitDate[1], splitDate[0], splitDate[2]);
            var stage = new StageController();
            var status = new StatusController();

            if (Name != null && Description != null && Price != null)
            {
                Project newProject = new Project
                {
                    Id = Guid.NewGuid(),
                    Name = Name,
                    Description = Description,
                    DateCreate = DateTime.Now,
                    DateComplete = DateTime.Parse(newDate),
                    Price = Price,
                    Stages = stage.Get("Анализ требований"),
                    Status = status.Get("В работе")                                     
                    
                };

                dbContext.Projects.Add(newProject);
                dbContext.SaveChanges();
                
            }

            return RedirectToAction("AddPage");
        }

        [Authorize(Roles = "Руководитель")]
        public ActionResult Edit(Guid Id)
        {
            var currentStage = dbContext.Stages.FirstOrDefault(stage => stage.Id == stage.Projects.Select(a => a.Stages.Id).FirstOrDefault());
            var currentStatus = dbContext.Status.FirstOrDefault(status => status.Id == status.Projects.Select(a => a.Status.Id).FirstOrDefault());
            
            if(currentStage != null && currentStatus != null)
            {
                ViewBag.Stage = dbContext.Stages.Where(a => a.Id != currentStage.Id).ToList();
                ViewBag.Status = dbContext.Status.Where(a => a.Id != currentStatus.Id).ToList();

                ViewBag.CurrentStage = currentStage.Name.ToString();
                ViewBag.CurrentStatus = currentStatus.Name.ToString();
            }

            SiteMaps.Current.CurrentNode.ParentNode.Title = dbContext.Projects.FirstOrDefault(t => t.Id == Id).Name;
            var edit = dbContext.Projects.FirstOrDefault(a => a.Id == Id);

            return View(edit);
        }


        [Authorize(Roles = "Руководитель")]
        [HttpPost]
        public bool UpdateMainInfo(Guid ProjectId, string name, string desc, string date, string price)
        {
            var newDate = date;

            if (name != null && desc != null && date != null && price != null)
            {
                var splitDate = date.Split('/');

                if (splitDate.Count() > 1)
                {
                    newDate =  String.Format("{0}.{1}.{2}", splitDate[1], splitDate[0], splitDate[2]);
                }

                var project = dbContext.Projects.FirstOrDefault(a => a.Id == ProjectId);

                if (project != null)
                {
                    project.Name = name;
                    project.Description = desc;
                    project.Price = price;
                    project.DateComplete = DateTime.Parse(newDate);

                    dbContext.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        [Authorize(Roles = "Руководитель")]
        [HttpPost]
        public bool UpdateStage(Guid StageId, Guid ProjectId)
        {
            var stage = dbContext.Stages.FirstOrDefault(a => a.Id == StageId);
            var project = dbContext.Projects.FirstOrDefault(a => a.Id == ProjectId);

            if (stage != null && project != null)
            {
                project.Stages = stage;

                dbContext.SaveChanges();

                return true;
            }

            return false;
        }

        [Authorize(Roles = "Руководитель")]
        [HttpPost]
        public bool UpdateStatus(Guid StatusId, Guid ProjectId)
        {
            var status = dbContext.Status.FirstOrDefault(a => a.Id == StatusId);
            var project = dbContext.Projects.FirstOrDefault(a => a.Id == ProjectId);

            if (status != null && project != null)
            {
                project.Status = status;

                dbContext.SaveChanges();

                return true;
            }

            return false;
        }

        [Authorize(Roles = "Руководитель")]
        [HttpGet]
        public RedirectToRouteResult Delete(Guid Id)
        {
            var select = dbContext.Projects.FirstOrDefault(a => a.Id == Id);
            var selectTasksProject = dbContext.Tasks.Where(a => a.Project.Id == Id).ToList();

            if (select != null)
            {
                foreach(var task in selectTasksProject)
                {
                    dbContext.Tasks.Remove(task);
                }

                dbContext.Projects.Remove(select); 
                dbContext.SaveChanges();

            }

            return RedirectToAction("Index");
        }
    }
}