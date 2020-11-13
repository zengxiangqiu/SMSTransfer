using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Requests
{
    public abstract class SmsRequest
    {
        public string appkey { get; set; }
        public string projectId { get; set; }
    }
}
