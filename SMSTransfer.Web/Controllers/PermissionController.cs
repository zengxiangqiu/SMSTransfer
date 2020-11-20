using SMSTransfer.Services;
using SMSTransfer.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSTransfer.Web.Controllers
{
    public class PermissionController : Controller
    {
        private readonly ISmsService _smsService;

        public PermissionController(ISmsService smsService)
        {
            this._smsService = smsService;
        }


        // GET: Permission
        public ActionResult Index()
        {
            var permissions = this._smsService.GetUserPermissions().GroupBy(x=>new { x.UserName, x.UserId }).Select(x=> new Permission { UserId =  x.Key.UserId, UserName =  x.Key.UserName,  Projects = string.Join(",",x.Select(y=>y.ProjectName).ToArray())});
            var vm = new PermissionViewModel
            {
                 Permissions = permissions
            };
            return View("Index", vm);
        }

        public ActionResult Edit(int? id )
        {
            var vm = this._smsService.GetUserPermissions()
                .Where(x => x.UserId == id)
                .GroupBy(x => new { x.UserId, x.UserName })
                .Select(x => new PerEditViewModel
                {
                    UserId = x.Key.UserId,
                    UserName = x.Key.UserName,
                    //if null
                    SelectedPers = x.Select(y => y.ProjectId.ToString()).ToArray()?? new string[] { },
                    Pers = this._smsService.GetProjects().Select(z => new SelectListItem { Text = z.ProjectName, Value = z.Id.ToString() })
                }).FirstOrDefault();
            return View("Edit", vm);
        }

        [HttpPost]
        public ActionResult Edit(PerEditViewModel viewModel)
        {
            var pers =  viewModel.SelectedPers.Select(x => new SMSTransfer.Models.SMSUserPermissions
            {
                ProjectId = int.Parse(x),
                UserId = viewModel.UserId
            });
           this._smsService.ResetPermissions(viewModel.UserId,pers);

            return RedirectToAction("Index");
        }
    }
}