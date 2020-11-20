using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSTransfer.Models;

namespace SMSTransfer.Web.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<SmsUser> Users { get; set; }
        public int PerPage { get; set; } = 10;
        public int? CurrentPage { get; set; } = 1;
        public int PageCount { get; set; }
    }
}