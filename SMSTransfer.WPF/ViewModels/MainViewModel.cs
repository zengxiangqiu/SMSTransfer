using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SMSTransfer.WPF.ViewModels
{
    using Models;
    public class MainViewModel: TaskBase
    {
        public string UserKey { get; set; } = "649cf2e3";
        private SmsTask _selectedTask;
        public SmsTask SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (_selectedTask != value)
                {
                    _selectedTask = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public   ObservableCollection<SmsTask>  SmsTasks { get; set; }

        /// <summary>
        /// 地区城市列表
        /// </summary>
        private Dictionary<string, List<string>> _areaWithCities = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> AreaWithCities
        {
            get => _areaWithCities;
            set
            {
                if (_areaWithCities != value)
                {
                    _areaWithCities = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("Areas");
                }
            }
        }

        /// <summary>
        /// 地区列表
        /// </summary>
        public List<string> Areas
        {
            get => AreaWithCities.Keys.ToList();
        }

        /// <summary>
        /// 选中的地区
        /// </summary>
        /// 

        private string _areaSelected;
        public string AreaSelected
        {
            get => _areaSelected;
            set
            {
                if (_areaSelected != value)
                {
                    _areaSelected = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("Cities");
                }
            }
        }

        private string _citySelected;
        public string CitySelected
        {
            get => _citySelected;
            set
            {
                if (_citySelected != value)
                {
                    _citySelected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 选中的城市
        /// </summary>
        public List<string> Cities
        {
            get => AreaWithCities[AreaSelected];
        }
    }
}
