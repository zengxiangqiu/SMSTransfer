using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Models
{
    public class SmsTelephone
    {
        public int Id { get; set; }

        public string Telephone { get; set; }

        public string Area { get; set; }

        public string City { get; set; }
    }
}
