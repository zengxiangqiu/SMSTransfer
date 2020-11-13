using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace SMSTransfer.Repositories
{
    using Models;
    using NLog;
    using SMSTransfer.Requests;
    using SMSTransfer.Responses;

    public  class SmsRemoteRepository: SmsBaseRepository, ISmsRepository
    {
        public SmsRemoteRepository(string appkey,string projectId, ILogger logger) :base(appkey, projectId, logger)
        {
           
        }

        public Task<Dictionary<string, List<string>>> GetAreasWithCitiesAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取号码接口
        /// </summary>
        /// <returns></returns>
        public override async Task<string> GetTelephoneAsync(string area, string city,string userKey)
        {
            var uri = "getPhoneNum";
            var request = new RestRequest(uri);
            var seg = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(new GetTelRequest { appkey=_appkey , projectId =_projectId }));
            foreach (var item in seg)
            {
                request.AddUrlSegment(item.Key, item.Value);
            }
            try
            {
                var resp = await client.GetAsync<SmsResponse>(request);
                if (resp.code != 100000)
                    throw new Exception(resp.msg);
                else
                    return resp.data.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //base.Disconnect();
            }
        }

        public Task<string> GetTelephoneAsync(string tel, string userKey)
        {
            //接口未开放
            throw new NotImplementedException();
        }
    }
}
