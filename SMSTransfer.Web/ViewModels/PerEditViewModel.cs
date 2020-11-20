using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSTransfer.Web.ViewModels
{
    public class PerEditViewModel
    {
        public string[] SelectedPers { get; set; }
        [DisplayName("已选项目")]
        public IEnumerable<SelectListItem> Pers { get; set; }
        public int UserId { get; set; }
        [DisplayName("用户名")]
        public string UserName { get; set; }
    }
}