using SMSTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSTransfer.Web.ViewModels
{
    public class SumViewModel
    {
        public IEnumerable<SmsSummary> Summaries { get; set; }
    }
}