using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSTransfer.WPF.Models
{
    public class SmsTask: TaskBase
    {
        private string tel = "";
        public string Tel
        {
            get => tel;
            set
            {
                if (tel != value)
                {
                    tel = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Upcode { get; set; }
        public string Upmobile { get; set; }

        private string msg = "";
        public string Msg
        {
            get => msg;
            set
            {
                if (msg != value)
                {
                    msg = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private TaskStatus taskStatus  = TaskStatus.ReadToGetTel;
        public TaskStatus TaskStatus
        {
            get => taskStatus;
            set
            {
                if (taskStatus != value)
                {
                    taskStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }

    public enum TaskStatus
    {
        Loading,
        ReadToGetTel,
        ReadToSendMsg
    }
}
