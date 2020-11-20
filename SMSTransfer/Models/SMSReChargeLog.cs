using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Models
{
     public class SMSReChargeLog
    {
        public int Id { get; set; }
        public string UserKey { get; set; }
        public int CurPoints { get; set; }
        public int AddPoints { get; set; }
        public string LastModBy { get; set; }
        public DateTime LastModTime { get; set; }
    }
}
