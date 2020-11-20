using SMSTransfer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSTransfer.Web.ViewModels;

namespace SMSTransfer.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ISmsService _smsService;

        public ProjectController(ISmsService smsService)
        {
            this._smsService = smsService;
        }

        // GET: Project
        public ActionResult Index()
        {
            var projects = this._smsService.GetProjects();
            var vm = new ProjectViewModel
            {
                Projects = projects
            };
            return View("Index", vm);
        }

        public ActionResult Edit(int? id)
        {
            var areas = (Dictionary<string, List<string>>)this._smsService.GetAreasWithCitiesAsync().Result.data;
            ProEditViewModel viewModel = null;
            if (id != null)
            {
                //修改
                viewModel = this._smsService.GetProjects().Where(x => x.Id == id).Select(x => new ProEditViewModel
                {
                    Id = x.Id,
                    ProjectName = x.ProjectName,
                    Areas = areas.Keys.Select(y => { var item = new SelectListItem(); item.Text = y; item.Value = y; return item; }),
                    SelectedAreas = x.Areas.Split(',').ToArray()
                }).FirstOrDefault();
            }
            else
            {
                viewModel = new ProEditViewModel
                {
                    Id = -1,
                    Areas = areas.Keys.Select(x => { var item = new SelectListItem(); item.Text = x; item.Value = x; return item; })
                };
            }
            return View("Edit", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(ProEditViewModel viewModel)
        {
            var area = string.Join(",", viewModel.SelectedAreas);
            var project = new SMSTransfer.Models.SMSProject
            {
                 Id = viewModel.Id,
                ProjectName = viewModel.ProjectName,
                Areas = area
            };
            this._smsService.InsertOrUpdateProject(project);
            var projects = this._smsService.GetProjects();
            var vm = new ProjectViewModel
            {
                Projects = projects
            };
            return RedirectToAction("Index", "Project");
        }
    }
}