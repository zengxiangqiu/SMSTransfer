using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SMSTransfer.Web.ViewModels
{
    public class NewViewModel
    {
        public int Id { get; set; }
        [DisplayName("密钥")]
        public string UserKey { get; set; }
        [DisplayName("用户名")]
        public string UserName { get; set; }
        //public string ProjectId { get; set; }
        [DisplayName("默认点数")]
        public int Points { get; set; }
    }
}