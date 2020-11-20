using SMSTransfer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSTransfer.Web.ViewModels;
using System.Threading.Tasks;

namespace SMSTransfer.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ISmsService _smsService;

        public CustomerController(ISmsService smsService)
        {
            this._smsService = smsService;
        }

        public ActionResult Index()
        {
            var usersVm = new UsersViewModel
            {
                Users = this._smsService.GetSmsUsers().ToList()
            };
            return View(usersVm);
        }

        public ActionResult Edit(int id)
        {
            var vm = this._smsService.GetSmsUsers().Where(x => x.Id == id).Select(x => new EditViewModel
            {
                Id = x.Id,
                UserKey = x.UserKey,
                UserName = x.UserName,
                CurPoints = x.Points,
                AddPoints = 0
            }).FirstOrDefault();

            return View(vm);
        }


        public ActionResult New()
        {
            SMSTransfer.Models.SmsUser isExist = null;
            string userKey = "";
            do
            {
                userKey = Guid.NewGuid().ToString().GetHashCode().ToString("x");
                isExist = this._smsService.GetSmsUsers().Where(x => x.UserKey == userKey).FirstOrDefault();
            } while (isExist != null);

            var vm = new SMSTransfer.Web.ViewModels.NewViewModel
            {
                UserKey = userKey,
            };

            return View("New", vm);
        }

        [HttpPost]
        public ActionResult New(NewViewModel viewModel)
        {
            if (viewModel != null)
            {
                this._smsService.SaveSmsUser(new SMSTransfer.Models.SmsUser
                {
                    UserKey = viewModel.UserKey,
                    Status = true,
                    CreateTime = DateTime.Now,
                    LastModTime = DateTime.Now,
                    Points = viewModel.Points,
                    //ProjectId = viewModel.ProjectId,
                    UserName = viewModel.UserName
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(EditViewModel user)
        {
            if (user != null)
            {
                this._smsService.ReCharge(user.UserKey, user.AddPoints);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ReChargeLog(string id)
        {
            var usersVm = new ReChargeLogViewModel
            {
               Logs = this._smsService.GetReChargeLogs()
            };
            if (id != null)
                usersVm.Logs =  usersVm.Logs.Where(x => x.UserKey == id);

            return View("ReChargeLog", usersVm);
        }


        public ActionResult Log(string id)
        {
            var usersVm = new LogViewModel
            {

                Logs = this._smsService.GetSmsLogs()
            };
            if (id != null)
                usersVm.Logs = usersVm.Logs.Where(x => x.UserKey == id);

            return View("Log", usersVm);
        }

        public ActionResult Summary()
        {
            var sumViwModel = new SumViewModel
            {
                 Summaries = this._smsService.GetSmsSummaries()
            };
            return View("Summary", sumViwModel);
        }

    }
}
