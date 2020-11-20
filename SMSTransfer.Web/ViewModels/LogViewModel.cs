using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSTransfer.Models;

namespace SMSTransfer.Web.ViewModels
{
    public class LogViewModel
    {
        public IEnumerable<SmsLog> Logs { get; set; }
    }
}