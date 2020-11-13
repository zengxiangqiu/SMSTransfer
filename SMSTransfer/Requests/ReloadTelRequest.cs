using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Requests
{
    public class ReloadTelRequest: SmsRequest
    {
        /// <summary>
        /// 拉黑手机号
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// type=1时为近一个月注册过~type=2时为未收到验证码~type=3时为成功发送
        /// </summary>
        public string type { get; set; }
    }
}
