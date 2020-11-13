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
        public SmsLogsRepository(string appkey, string projectId,ILogger logger) : base(appkey, projectId)
        {
            this._logger = logger;
        }

        public async Task InsertLogAsync(string userKey, string telephone)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();

            try
            {
                var smtParams = new { USERKEY = userKey, TELEPHONE = telephone, CREATETIME = DateTime.Now.Date, LASTMODTIME = DateTime.Now };
                var log = con.QueryFirstOrDefault<SmsLog>("SELECT * FROM SMSLogs WHERE USERKEY=@USERKEY AND TELEPHONE=@TELEPHONE AND CREATETIME=@CREATETIME;", smtParams);

                if (log is null)
                {
                    var result = -1;
                     result = await con.ExecuteAsync("INSERT INTO SMSLogs(USERKEY,TELEPHONE,COUNTOFSENT,CREATETIME,LASTMODTIME)VALUES(@USERKEY,@TELEPHONE,0,@CREATETIME,@LASTMODTIME);", smtParams);
                    if (result != 1)
                    {
                        //记录日志失败
                        this._logger.Debug($"{userKey} 使用 {telephone} 占用号码失败");
                    }
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

        public async Task UpdateLogAsync(string userKey, string telephone)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();

            try
            {
                var smtParams = new { USERKEY = userKey, TELEPHONE = telephone, CREATETIME = DateTime.Now.Date, LASTMODTIME = DateTime.Now };

                var result = await con.ExecuteAsync("UPDATE SMSLogs SET COUNTOFSENT = COUNTOFSENT+1,LASTMODTIME = @LASTMODTIME WHERE CREATETIME=@CREATETIME  AND USERKEY = @USERKEY AND TELEPHONE=@TELEPHONE;", smtParams);

                if (result != 1)
                {
                    //记录日志失败
                    this._logger.Debug($"{userKey} 使用 {telephone} 后计数失败");
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
    }
}
