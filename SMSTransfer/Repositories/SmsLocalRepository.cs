using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SQLite;


namespace SMSTransfer.Repositories
{
    using Models;
    using NLog;

    public class SmsLocalRepository : SmsBaseRepository, ISmsRepository
    {
        private readonly ILogger _logger;
        public SmsLocalRepository(string appkey, string projectId, ILogger logger) : base(appkey, projectId, logger)
        {
            this._logger = logger;
        }

        public async Task<Dictionary<string, List<string>>> GetAreasWithCitiesAsync()
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                var areas = await con.QueryAsync<KeyValuePair<string, string>>("SELECT AREA as Key,CITY as Value FROM SMSTELEPHONES GROUP BY AREA,CITY;");

                var result = areas.GroupBy(x => x.Key).ToDictionary(x => x.Key,y=>y.Select(x=>x.Value).ToList());

                this._logger.Debug("获取地区城市信息");

                return result;
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

        public override async Task<string> GetTelephoneAsync(string area, string city, string userKey)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                var telephone = await con.QueryFirstOrDefaultAsync<SmsTelephone>("SELECT TEL.* FROM SMSTELEPHONES TEL WHERE NOT EXISTS(SELECT * FROM SMSLogs LOG WHERE  TEL.TELEPHONE =LOG.TELEPHONE AND CREATETIME = @CREATETIME AND USERKEY=@USERKEY) AND AREA=@AREA AND CITY = @CITY LIMIT 1",
                new
                {
                    USERKEY = userKey,
                    CREATETIME = DateTime.Now.Date,
                    AREA = area,
                    CITY = city
                });

                this._logger.Debug("获取号码池号码" + telephone?.Telephone ?? "");


                if (telephone is null)
                    throw new Exception("获取不到对应地级市的手机号");
                else
                    return telephone.Telephone;
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

        public async Task<string> GetTelephoneAsync(string tel, string userKey)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            this._logger.Debug("获取指定号码：" + tel);
            try
            {
                var telephone = await con.QueryFirstOrDefaultAsync<SmsTelephone>(@"SELECT TEL.*
FROM SMSTELEPHONES TEL
WHERE EXISTS (
		SELECT 1
		FROM SMSLogs LOG
		WHERE LOG.USERKEY =@USERKEY
			AND LOG.CREATETIME = @CREATETIME
			AND LOG.TELEPHONE = TEL.TELEPHONE
		) AND TEL.TELEPHONE=@TELEPHONE LIMIT 1 ;",
                new
                {
                    USERKEY = userKey,
                    CREATETIME = DateTime.Now.Date,
                    TELEPHONE = tel,
                });

                if (telephone is null)
                    throw new Exception("获取不到指定的号码");
                else
                    return telephone.Telephone;
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
