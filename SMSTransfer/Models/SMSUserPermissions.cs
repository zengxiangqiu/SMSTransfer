using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Models
{
    public class SMSUserPermissions
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ProjectName { get; set; }
    }
}
