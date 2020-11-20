using SMSTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NLog;

namespace SMSTransfer.Repositories
{
    public class SmsLogsRepository : BaseRepository
    {
        private readonly ILogger _logger;
        public SmsLogsRepository(string appkey, string projectId, ILogger logger) : base(appkey, projectId)
        {
            this._logger = logger;
        }

        public async Task InsertLogAsync(string userKey, string telephone, string targetPhone = "", string content = "", int points = 0)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();

            try
            {
                var smtParams = new { USERKEY = userKey, TELEPHONE = telephone, POINTS = points, CREATETIME = DateTime.Now.Date, LASTMODTIME = DateTime.Now, TARGETPHONE = targetPhone, CONTENT = content };
                var result = -1;
                result = await con.ExecuteAsync("INSERT INTO SMSLogs(USERKEY,TELEPHONE,TARGETPHONE,CONTENT,POINTS,CREATETIME,LASTMODTIME)VALUES(@USERKEY,@TELEPHONE,@TARGETPHONE,@CONTENT,@POINTS,@CREATETIME,@LASTMODTIME);", smtParams);
                if (result != 1)
                {
                    //记录日志失败
                    this._logger.Debug($"{userKey} 使用 {telephone} 记log失败");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //base.Disconnect();
            }
        }

        public IEnumerable<SMSReChargeLog> GetReChargeLogs()
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                var user = con.Query<SMSReChargeLog>("select * from  SMSReChargeLog;");
                return user;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //base.Disconnect();
            }
        }

        public IEnumerable<SmsLog> GetSmsLogs()
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                var user = con.Query<SmsLog>("select * from  SMSLogs;");
                return user;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //base.Disconnect();
            }
        }

        public IEnumerable<SmsSummary> GetSmsSummaries()
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                var user = con.Query<SmsSummary>("SELECT SUM(LOG.POINTS) AS POINTS, USER.USERKEY,USER.USERNAME FROM SMSLogs LOG INNER JOIN SMSUsers USER ON LOG.USERKEY=USER.USERKEY  WHERE LOG.POINTS>0 GROUP BY USER.USERKEY,USER.USERNAME;");
                return user;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //base.Disconnect();
            }
        }

        public void InsertReChargeLog(SMSReChargeLog log)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                con.Execute("INSERT INTO SMSReChargeLog(USERKEY,CURPOINTS,ADDPOINTS,LASTMODBY,LASTMODTIME)VALUES(@USERKEY,@CURPOINTS,@ADDPOINTS,@LASTMODBY,@LASTMODTIME);", log);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //base.Disconnect();
            }
        }
    }
}
