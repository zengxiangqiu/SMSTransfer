using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Responses
{
    public class SmsResponse
    {
        /// <summary>
        /// code为100000代表获取成功,其余为失败
        /// </summary>
        public int code { get; set; } = 100000;

        public string msg { get; set; }

        public object data { get; set; }
    }

    public static class Message
    {
        public  readonly static  string Success = "成功";
        public  readonly static  string Failure = "失败";
        public  readonly static  int SuccessCode = 100000;
    }
}
