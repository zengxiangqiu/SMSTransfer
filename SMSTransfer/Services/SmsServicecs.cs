using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Services
{
    using Models;
    using NLog;
    using Repositories;
    using SMSTransfer.Responses;

    public class SmsService : ISmsService
    {
        private readonly ISmsRepository _smsRepository;
        private readonly SmsUserRepository _userRepository;
        private readonly SmsLogsRepository _logRepository;
        private readonly ILogger _logger;

        public SmsService(ISmsRepository smsBaseRepository, SmsUserRepository smsUserRepository, SmsLogsRepository logsRepository, ILogger logger)
        {
            this._smsRepository = smsBaseRepository;
            this._userRepository = smsUserRepository;
            this._logRepository = logsRepository;
            this._logger = logger;
        }

        public async Task<SmsResponse> GetAreasWithCitiesAsync()
        {
            try
            {
                var areas = await this._smsRepository.GetAreasWithCitiesAsync();
                return new SmsResponse { code = 100000, data = areas, msg = Message.Success };
            }
            catch (Exception ex)
            {
                this._logger.Debug(ex,"获取地区城市信息失败");
                return new SmsResponse { code = -1, data = new Dictionary<string,List<string>>(), msg = ex.Message };
            }
        }

        public async Task<SmsResponse> GetSmsTelephone(string area, string city, string tel, string userKey)
        {
            try
            {
                var user = await this._userRepository.GetSmsUserAsync(userKey);

                string telephone;

                if (tel == string.Empty)
                    telephone = await this._smsRepository.GetTelephoneAsync(area, city, user.UserKey);
                else
                    telephone = await this._smsRepository.GetTelephoneAsync(tel, user.UserKey);

                await this._logRepository.InsertLogAsync(user.UserKey, telephone);

                this._logger.Debug($"{userKey} 已获取 {area} | {city} 或指定号码 {telephone}");

                return new SmsResponse { code = 100000, data = telephone, msg = Message.Success };
            }
            catch (Exception ex)
            {
                this._logger.Debug($"{userKey} 取号{tel}异常,{ex.Message}");
                return new SmsResponse { code = -1, data = "", msg = ex.Message };
            }
        }

        public async Task<SmsResponse> SendMsgAsync(string tel, string upcode, string upmobile, string userKey)
        {
            try
            {
                var user = await this._userRepository.GetSmsUserAsync(userKey);

                //await this._smsRepository.SendMsgAsync(tel, upcode, upmobile, user.UserKey);

                await this._logRepository.UpdateLogAsync(user.UserKey, tel);

                //剩余点数
                var odd = await this._userRepository.DeductionAsync(user);

                this._logger.Debug($"{userKey} 使用 {tel} 发送 {upcode}  到 {upmobile} ");

                return new SmsResponse { code = 100000, data =new { odd = odd }, msg = Message.Success };
            }
            catch (Exception ex)
            {
                this._logger.Error(ex,$"{userKey}  {tel} 发送短信至 {upmobile} 失败");
                return new SmsResponse { code = -1, data  = new { odd = 0}, msg = ex.Message };
            }
        }
    }

    public interface ISmsService
    {
        Task<SmsResponse> GetSmsTelephone(string area, string city, string tel, string userKey);
        Task<SmsResponse> SendMsgAsync(string tel, string upcode, string upmobile, string userKey);
        Task<SmsResponse> GetAreasWithCitiesAsync();
    }
}
