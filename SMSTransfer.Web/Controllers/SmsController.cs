using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SMSTransfer.Web.Controllers
{
    using SMSTransfer.Services;
    using System.Web.Http.Results;
    using SMSTransfer.Responses;
    using SMSTransfer.Requests;

    public class SmsController : ApiController
    {
        private readonly ISmsService _smsService;
        public SmsController(ISmsService smsService)
        {
            this._smsService = smsService;
        }

        public async Task<IHttpActionResult> GetTelephone(string id)
        {
            var kv = Request.RequestUri.ParseQueryString();
            var area = kv["area"] ?? "";
            var city = kv["city"] ?? "";
            var tel = kv["tel"] ?? "";

            var response = await this._smsService.GetSmsTelephone(area, city, tel, id);

            return Ok(response);
        }

        [Route("api/sms/areas")]
        public async Task<IHttpActionResult> GetAreasAsync()
        {
            var response = await this._smsService.GetAreasWithCitiesAsync();
            return Ok(response);
        }


        public async Task<IHttpActionResult> PostMsgAsync(string id, [FromBody]SendMsgRequest request)
        {
            var response = await this._smsService.SendMsgAsync(request.tel, request.upcode, request.upmobile, id);
            return Ok(response);
        }

    }
}
