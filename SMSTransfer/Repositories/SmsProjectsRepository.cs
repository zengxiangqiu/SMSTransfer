using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SMSTransfer.Models;

namespace SMSTransfer.Repositories
{
    public class SmsProjectsRepository : BaseRepository
    {
        public SmsProjectsRepository(string appkey, string projectId) : base(appkey, projectId)
        {
        }

        public void AddProject(SMSProject project)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                 con.Execute("INSERT INTO SMSProjects(PROJECTNAME,AREAS)VALUES(@PROJECTNAME,@AREAS);", project);
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

        public void UpdateProject(SMSProject project)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                con.Execute("UPDATE SMSProjects SET PROJECTNAME=@PROJECTNAME,AREAS=@AREAS WHERE ID=@ID;", project);
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

        public IEnumerable<SMSProject> GetProjects()
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
            try
            {
                var projects = con.Query<SMSProject>("SELECT * FROM  SMSProjects;");
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
    }
}
