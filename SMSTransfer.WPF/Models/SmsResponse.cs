using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.WPF.Models
{
    public abstract class BaseResponse
    {
        /// <summary>
        /// code为100000代表获取成功,其余为失败
        /// </summary>
        public int code { get; set; } = 100000;

        public string msg { get; set; }
    }

    public class SmsResponse: BaseResponse
    {
        public string data { get; set; }
    }

    public class SendMsgResponse : BaseResponse
    {
        public SendMsgData data { get; set; }
    }

    public class GetAreasResponse : BaseResponse
    {
        public Dictionary<string, List<string>> data { get; set; }
    }

    public static class ResponseMsg
    {
        public static int SucessCode = 100000;
    }

    public class SendMsgData
    {
        public int Odd { get; set; }
    }

}
