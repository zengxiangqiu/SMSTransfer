using SMSTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SMSTransfer.Repositories
{
    public class SmsPermisRepository : BaseRepository
    {
        public SmsPermisRepository(string appkey, string projectId) : base(appkey, projectId)
        {
        }


        public IEnumerable<SMSUserPermissions> GetPermissions()
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                var projects = con.Query<SMSUserPermissions>("SELECT  PER.ID, PRO.ID AS PROJECTID,USER.ID AS USERID,USER.USERNAME,PRO.PROJECTNAME FROM SMSUsers USER LEFT JOIN SMSUserPermissions PER ON USER.ID = PER.USERID LEFT JOIN SMSProjects PRO ON PRO.ID= PER.PROJECTID;");
                return projects;
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

        public void AddUserPermission(SMSUserPermissions permission)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                con.Execute("INSERT INTO SMSUserPermissions(PROJECTID,USERID)VALUES(@PROJECTID,@USERID);", permission);
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

        public void Clear(int userId)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                con.Execute("DELETE FROM  SMSUserPermissions  WHERE USERID=@USERID;", new { USERID = userId });
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
