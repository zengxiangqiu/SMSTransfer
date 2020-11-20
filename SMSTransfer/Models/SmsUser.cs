using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Models
{
    public class SmsUser
    {
        public int Id { get; set; }
        public string UserKey { get; set; }
        public string UserName { get; set; }
        public string ProjectId { get; set; } = "";
        public int Points { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreateTime { get; set; }
        public DateTime LastModTime { get; set; }
    }
}
