using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Models
{
    public class SmsLog
    {
        public int Id { get; set; }
        public string UserKey { get; set; }
        public string Telephone { get; set; }
        public int CountOfSent { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastModTime { get; set; }
        
    }
}
