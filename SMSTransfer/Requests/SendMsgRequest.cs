using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Requests
{
    public class SendMsgRequest: SmsRequest
    {
        public string tel { get; set; }
        public string upcode { get; set; }
        public string upmobile { get; set; }
    }

}
