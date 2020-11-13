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

                HasPermission(user);

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

        protected void HasPermission(SmsUser user)
        {
            if (user is null)
                throw new Exception("密钥无效");
            else if (user.Status != 0)
                throw new Exception("密钥已锁定");
            else if (user.LimitedQty <= 0)
                throw new Exception("点数为0，无法扣点");
        }

        /// <summary>
        /// 扣点
        /// </summary>
        /// <param name="smsUser"></param>
        /// <returns>剩余点数</returns>
        public async  Task<int> DeductionAsync(SmsUser smsUser)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();

            try
            {
                var result = await con.ExecuteAsync("update SMSUsers set LIMITEDQTY =LIMITEDQTY-1  where USERKEY = @USERKEY and LIMITEDQTY>0;", new { USERKEY = smsUser.UserKey });
                if (result != 1)
                    throw new Exception("扣点失败");
                return smsUser.LimitedQty--;
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
