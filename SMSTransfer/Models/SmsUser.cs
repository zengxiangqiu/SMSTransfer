﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.Models
{
    public class SmsUser
    {
        public int Id { get; set; }
        public string UserKey { get; set; }
        public int LimitedQty { get; set; }
        public int Status { get; set; }
    }
}
