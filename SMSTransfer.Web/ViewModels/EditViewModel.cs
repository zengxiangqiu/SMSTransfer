using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using SMSTransfer.Models;

namespace SMSTransfer.Web.ViewModels
{
    public class EditViewModel
    {
        public int Id { get; set; }
        [DisplayName("密钥")]
        public string UserKey { get; set; }
        [DisplayName("用户名")]
        public string UserName { get; set; }

        [DisplayName("当前点数")]
        public int CurPoints { get; set; }

        [DisplayName("充值")]
        public int AddPoints { get; set; } = 0;
    }
}