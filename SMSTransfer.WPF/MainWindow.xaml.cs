using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMSTransfer.WPF
{
     using ViewModels;
    using Services;
    using Models;
    using ControlzEx.Theming;

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow :Window
    {
        private readonly SmsService service;
        public MainWindow()
        {
            InitializeComponent();
            //ThemeManager.Current.ChangeTheme(this, "Dark.Red");
            service = new SmsService();
           var vm =  new MainViewModel
            {
                SmsTasks = new System.Collections.ObjectModel.ObservableCollection<Models.SmsTask> {
                    //new Models.SmsTask{ }
                }
            };

            //vm.SelectedTask = vm.SmsTasks[0];
            this.DataContext = vm;

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            vm.AreaWithCities = await service.GetAreasAsync();
        }

        private async void BtnGetTel_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            var task = dgTasks.SelectedItem as SmsTask;
            try
            {
                task.Msg = "正在获取号码，请稍等";
                task.TaskStatus = TaskStatus.Loading;
                task.Tel = await this.service.GetTelephoneAsync(vm.AreaSelected, vm.CitySelected, task.Tel, vm.UserKey);
                task.Msg = "已成功获取号码";
                task.TaskStatus = TaskStatus.ReadToSendMsg;
            }
            catch (Exception ex)
            {
                task.Msg = "获取失败，" + ex.Message;
                task.TaskStatus = TaskStatus.ReadToGetTel;
            }
        } 

        private async void BtnSendMsg_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            var task = dgTasks.SelectedItem as SmsTask;
            try
            {
                task.Msg = "正在发送短信，请稍等";
                task.TaskStatus = TaskStatus.Loading;
                var odd = await this.service.SendMsgAsync(task.Tel, task.Upcode, task.Upmobile, vm.UserKey);
                task.Msg = "已成功发送短信，剩余点数" + odd;
            }
            catch (Exception ex)
            {
                task.Msg = "发送失败，" + ex.Message;
            }
            finally
            {
                task.TaskStatus = TaskStatus.ReadToSendMsg;
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            if (vm.UserKey.Trim() == "" || vm.AreaSelected  is null|| vm.CitySelected is null)
            {
                MessageBox.Show("请完善密钥、地区和城市","提示");
                return;
            }
            vm.SmsTasks.Add(new SmsTask());
        }
    }
}
