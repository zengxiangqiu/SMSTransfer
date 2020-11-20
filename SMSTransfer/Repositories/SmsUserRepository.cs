using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SMSTransfer.Repositories
{
    using Models;
    using System.Data.SQLite;

    public class SmsUserRepository : BaseRepository
    {
        public SmsUserRepository(string appkey, string projectId) : base(appkey, projectId)
        {
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<SmsUser> GetSmsUserAsync(string userKey)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                var user = await con.QueryFirstOrDefaultAsync<SmsUser>("select * from  SMSUsers where USERKEY = @USERKEY;", new { USERKEY = userKey });

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

        /// <summary>
        /// 扣点
        /// </summary>
        /// <param name="smsUser"></param>
        /// <returns>剩余点数</returns>
        public async Task<int> DeductionAsync(SmsUser smsUser)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();

            try
            {
                var result = await con.ExecuteAsync("update SMSUsers set POINTS =POINTS-1  where USERKEY = @USERKEY and POINTS>0;", new { USERKEY = smsUser.UserKey });
                if (result != 1)
                    throw new Exception("扣点失败");
                return --smsUser.Points;
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

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="pageSize">页码大小</param>
        /// <returns></returns>
        public IEnumerable<SmsUser> GetSmsUsers()
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                var user =  con.Query<SmsUser>("select * from  SMSUsers;");
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

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="points">点数</param>
        public void ReCharge(SmsUser user, int points)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                con.Execute("UPDATE SMSUsers SET POINTS = @POINTS WHERE USERKEY = @USERKEY;" ,new { POINTS = points+user.Points, USERKEY = user.UserKey });
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

        public SmsUser New(SmsUser user)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                con.Execute("INSERT INTO SMSUsers(USERKEY, USERNAME,PROJECTID,POINTS,STATUS,CREATETIME,LASTMODTIME)VALUES(@USERKEY,@USERNAME,@PROJECTID,@POINTS,@STATUS,@CREATETIME,@LASTMODTIME);", user);

                var newUser = GetSmsUserAsync(user.UserKey).Result;
                return newUser;
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
