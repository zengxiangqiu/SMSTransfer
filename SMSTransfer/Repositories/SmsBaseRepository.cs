using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SMSTransfer.Repositories
{
    using Models;
    using NLog;
    using RestSharp;
    using SMSTransfer.Requests;
    using SMSTransfer.Responses;

    public abstract class SmsBaseRepository: BaseRepository
    {
        private readonly string host = "http://39.98.79.162:8989/api/phone/";
        protected readonly IRestClient client;

        public SmsBaseRepository(string appkey, string projectId, ILogger logger):base(appkey,projectId)
        {
            client = new RestClient(host);
        }

        private async Task<string> HasGotTelAsync(string tel, string userKey)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();

            var log = await con.QueryFirstOrDefaultAsync<SmsLog>("select * from SMSLogs where USERKEY = @USERKEY and TELEPHONE=@TELEPHONE and CREATETIME=@CREATETIME;", new
            {
                TELEPHONE = tel,
                USERKEY = userKey,
                CREATETIME = DateTime.Now.Date
            });

            if (log is null)
                throw new Exception("请先获取号码再发送");
            else
                return log.Telephone;
        }


        /// <summary>
        /// 发送短信接口
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="upcode"></param>
        /// <param name="upmobile"></param>
        /// <returns>剩余点数</returns>
        public async Task SendMsgAsync(string tel, string upcode, string upmobile, string userKey)
        {
            await HasGotTelAsync(tel, userKey);

            var uri = "sendMsg";
            var request = new RestRequest(uri);
            request.AddJsonBody(new SendMsgRequest
            {
                appkey = _appkey,
                projectId = _projectId,
                tel = tel,
                upcode = upcode,
                upmobile = upmobile
            });
            try
            {
                var resp = await client.PostAsync<SmsResponse>(request);
                if (resp.code != Message.SuccessCode)
                    throw new Exception("发送失败");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Disconnect();
            }
        }

        /// <summary>
        /// 拉黑电话号码
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<SmsResponse> ReloadTelephone(string tel, string type)
        {
            var uri = "reloadTel";
            var request = new RestRequest(uri);
            request.AddJsonBody(new ReloadTelRequest
            {
                appkey = _appkey,
                projectId = _projectId,
                tel = tel,
                type = type
            });
            var resp = client.PostAsync<SmsResponse>(request);
            return resp;
        }

        public abstract Task<string> GetTelephoneAsync(string area, string city, string userKey);
    }

    public abstract class BaseRepository:IDisposable
    {
        protected readonly string _appkey, _projectId;
        protected SQLiteConnection con = new SQLiteConnection($"Data Source = {System.Web.HttpContext.Current.Request.MapPath("~\\Database\\sms.sqlite")};Version=3;");

        public BaseRepository(string appkey, string projectId)
        {
            _appkey = appkey;
            _projectId = projectId;
        }

        public void Dispose()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }

    public interface ISmsRepository
    {
        Task<string> GetTelephoneAsync(string area, string city, string userKey);
        Task<string> GetTelephoneAsync(string tel, string userKey);
        Task SendMsgAsync(string tel, string upcode, string upmobile, string userKey);
        Task<Dictionary<string, List<string>>> GetAreasWithCitiesAsync();
    }
}
