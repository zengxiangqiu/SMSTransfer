using SMSTransfer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SMSTransfer.Web.ViewModels
{
    public class PermissionViewModel
    {
        public IEnumerable<Permission> Permissions { get; set; }
    }

    public class Permission
    {
        public int UserId { get; set; }
        [DisplayName("用户名")]
        public string UserName { get; set; }

        [DisplayName("项目编号")]
        public string Projects { get; set; }
    }
}