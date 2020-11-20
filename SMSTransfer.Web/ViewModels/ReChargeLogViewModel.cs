using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using SMSTransfer.Models;

namespace SMSTransfer.Web.ViewModels
{
    public class ReChargeLogViewModel
    {
        public IEnumerable<SMSReChargeLog> Logs { get; set; }
    }
}