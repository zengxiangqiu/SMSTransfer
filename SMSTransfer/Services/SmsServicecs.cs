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
        private readonly SmsProjectsRepository _proRepository;
        private readonly SmsPermisRepository _perRepository;
        private readonly ILogger _logger;

        public SmsService(ISmsRepository smsBaseRepository, SmsUserRepository smsUserRepository
            , SmsLogsRepository logsRepository
            , SmsProjectsRepository proRepository
            , SmsPermisRepository perRepository
            , ILogger logger)
        {
            this._smsRepository = smsBaseRepository;
            this._userRepository = smsUserRepository;
            this._logRepository = logsRepository;
            this._proRepository = proRepository;
            this._perRepository = perRepository;
            this._logger = logger;
        }

        public async Task<SmsResponse> GetAreasWithCitiesAsync(string userKey="")
        {
            try
            {
                var areas = await this._smsRepository.GetAreasWithCitiesAsync(userKey);
                return new SmsResponse { code = 100000, data = areas, msg = Message.Success };
            }
            catch (Exception ex)
            {
                this._logger.Debug(ex, "获取地区城市信息失败");
                return new SmsResponse { code = -1, data = new Dictionary<string, List<string>>(), msg = ex.Message };
            }
        }

        public async Task<SmsResponse> GetSmsTelephone(string area, string city, string tel, string userKey)
        {
            try
            {
                var user = await this._userRepository.GetSmsUserAsync(userKey);

                HasPermission(user);

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

        public IEnumerable<SmsUser> GetSmsUsers()
        {
            try
            {
                var users = this._userRepository.GetSmsUsers();
                return users;
            }
            catch (Exception ex)
            {
                this._logger.Debug(ex, $"获取用户信息失败");
                return new List<SmsUser>();
            }
        }

        public void ReCharge(string userKey, int points)
        {
            try
            {
                var user = this._userRepository.GetSmsUserAsync(userKey).Result;
                this._userRepository.ReCharge(user, points);
                //log
                this._logRepository.InsertReChargeLog(new SMSReChargeLog
                {
                    UserKey = user.UserKey,
                    CurPoints = user.Points,
                    AddPoints = points,
                    LastModBy = "admin",
                    LastModTime = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, $"{userKey} 充值 {points} 失败");
            }
        }

        public async Task<SmsResponse> SendMsgAsync(string tel, string upcode, string upmobile, string userKey)
        {
            try
            {
                var user = await this._userRepository.GetSmsUserAsync(userKey);

                HasPermission(user);
                await this._smsRepository.SendMsgAsync(tel, upcode, upmobile, user.UserKey);

                await this._logRepository.InsertLogAsync(user.UserKey, tel, upmobile, upcode,1);

                //剩余点数
                var odd = await this._userRepository.DeductionAsync(user);

                this._logger.Debug($"{userKey} 使用 {tel} 发送 {upcode}  到 {upmobile} ");

                return new SmsResponse { code = 100000, data = new { odd = odd }, msg = Message.Success };
            }
            catch (Exception ex)
            {
                await this._logRepository.InsertLogAsync(userKey, tel, upmobile, upcode, 0);
                this._logger.Error(ex, $"{userKey}  {tel} 发送短信至 {upmobile} 失败");
                return new SmsResponse { code = -1, data = new { odd = 0 }, msg = ex.Message };
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public SmsUser SaveSmsUser(SmsUser user)
        {
            try
            {
                var newUser =  this._userRepository.New(user);
                return newUser;
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, $"新增用户{user.UserKey} 失败");
                return null;
            }
        }

        public IEnumerable<SMSReChargeLog> GetReChargeLogs()
        {
            try
            {
                var logs = this._logRepository.GetReChargeLogs();
                return logs;
            }
            catch (Exception ex)
            {
                this._logger.Debug(ex, $"获取充值记录失败");
                return new List<SMSReChargeLog>();
            }
        }

        protected void HasPermission(SmsUser user)
        {
            if (user is null)
                throw new Exception("密钥无效");
            else if (user.Status != true)
                throw new Exception("密钥已锁定");
            else if (user.Points <= 0)
                throw new Exception("点数为0，无法扣点");
        }

        public IEnumerable<SmsLog> GetSmsLogs()
        {
            try
            {
                var logs = this._logRepository.GetSmsLogs();
                return logs;
            }
            catch (Exception ex)
            {
                this._logger.Debug(ex, $"获取发送短信记录失败");
                return new List<SmsLog>();
            }
        }

        public IEnumerable<SMSProject> GetProjects()
        {
            try
            {
                var projects = this._proRepository.GetProjects();
                return projects;
            }
            catch (Exception ex)
            {
                this._logger.Debug(ex, $"获取项目失败");
                return new List<SMSProject>();
            }
        }

        public void InsertOrUpdateProject(SMSProject project)
        {
            try
            {
                if (project.Id == -1)
                    this._proRepository.AddProject(project);
                else
                    this._proRepository.UpdateProject(project);
            }
            catch (Exception ex)
            {
                this._logger.Debug(ex, $"新增或更新项目{project.ProjectName}失败");
            }
        }

        public IEnumerable<SMSUserPermissions> GetUserPermissions()
        {
            try
            {
                var permissions = this._perRepository.GetPermissions();
                return permissions;
            }
            catch (Exception ex)
            {
                this._logger.Debug(ex, $"获取权限设置失败");
                return new List<SMSUserPermissions>();
            }
        }

        public void ResetPermissions(int userId, IEnumerable<SMSUserPermissions> permissions)
        {
            try
            {
                 this._perRepository.Clear(userId);
                permissions.ToList().ForEach(this._perRepository.AddUserPermission);
            }
            catch (Exception ex)
            {
                this._logger.Debug(ex, $"修改权限设置失败");
            }
        }

        public IEnumerable<SmsSummary> GetSmsSummaries()
        {
            try
            {
                var summaries = this._logRepository.GetSmsSummaries();
                return summaries;
            }
            catch (Exception ex)
            {
                this._logger.Debug(ex, $"获取汇总失败");
                return new List<SmsSummary>();
            }
        }
    }

    public interface ISmsService
    {
        Task<SmsResponse> GetSmsTelephone(string area, string city, string tel, string userKey);
        Task<SmsResponse> SendMsgAsync(string tel, string upcode, string upmobile, string userKey);
        Task<SmsResponse> GetAreasWithCitiesAsync(string userKey = "");
        IEnumerable<SmsUser> GetSmsUsers();
        IEnumerable<SMSReChargeLog> GetReChargeLogs();
        IEnumerable<SmsLog> GetSmsLogs();
        IEnumerable<SmsSummary> GetSmsSummaries();
        void ReCharge(string userKey, int points);
        SmsUser SaveSmsUser(SmsUser user);

        IEnumerable<SMSProject> GetProjects();
        void InsertOrUpdateProject(SMSProject project);

        IEnumerable<SMSUserPermissions> GetUserPermissions();
        void ResetPermissions(int userId,IEnumerable<SMSUserPermissions> permissions);

    }
}
