using SMSTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSTransfer.Web.ViewModels
{
    public class ProjectViewModel
    {
        public IEnumerable<SMSProject> Projects { get; set; }

    }
}