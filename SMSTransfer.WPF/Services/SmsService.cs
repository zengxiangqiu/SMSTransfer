using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace SMSTransfer.WPF.Services
{
    using Models;
    public class SmsService
    {
        private readonly string host = "http://localhost:8099/api/sms/";
        IRestClient client;

        public SmsService()
        {
            client = new RestClient(host);
        }

        /// <summary>
        /// 获取号码
        /// </summary>
        /// <param name="area"></param>
        /// <param name="city"></param>
        /// <param name="tel"></param>
        /// <param name="userKey"></param>
        /// <returns>已获取的号码</returns>
        public async Task<string> GetTelephoneAsync(string area,string city,string tel, string userKey)
        {
            var request = new RestRequest(userKey);

            request.AddParameter("area", area ?? "");
            request.AddParameter("city", city ?? "");
            request.AddParameter("tel", tel ?? "");

            var response =  await client.GetAsync<SmsResponse>(request);

            if (response.code != ResponseMsg.SucessCode)
                throw new Exception(response.msg);

            return response.data;
        }

        /// <summary>
        /// 获取地区城市信息
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string,List<string>>> GetAreasAsync(string userKey)
        {
            var request = new RestRequest($"areas/{userKey}");

            var response = await client.GetAsync<GetAreasResponse>(request);

            if (response.code != ResponseMsg.SucessCode)
                throw new Exception(response.msg);

            return response.data;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="upcode"></param>
        /// <param name="upmobile"></param>
        /// <param name="userKey"></param>
        /// <returns>剩余点数</returns>
        public async Task<int> SendMsgAsync(string tel, string upcode, string upmobile, string userKey)
        {
            var request = new RestRequest(userKey);
            request.AddJsonBody(new { tel , upcode, upmobile });

            var response = await client.PostAsync<SendMsgResponse>(request);

            if (response.code != ResponseMsg.SucessCode)
                throw new Exception(response.msg);

            return response.data.Odd;
        }

    }
}
