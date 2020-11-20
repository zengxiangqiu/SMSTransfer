using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSTransfer.Web.ViewModels
{
    public class ProEditViewModel
    {
        public string[] SelectedAreas { get; set; } 
        [DisplayName("已选地区")]
        public IEnumerable<SelectListItem> Areas { get; set; }
        public int Id { get; set; }
        [DisplayName("项目名称")]
        public string ProjectName { get; set; }
    }
}