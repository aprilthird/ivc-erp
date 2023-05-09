using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.HrWorker.DailyTask;
using IVC.PE.BINDINGRESOURCES.Areas.HrWorker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.HrWorker.DailyTask
{
    public class WorkerDailyTaskListViewModel : BaseViewModel
    {
        #region IsRefreshing
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region IsEnabled
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region Workers
        private ObservableCollection<WorkerDailyTaskResourceModel> workers;
        public ObservableCollection<WorkerDailyTaskResourceModel> Workers
        {
            get { return this.workers; }
            set { this.SetValue(ref this.workers, value); }
        }
        private async void LoadWorkers()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<WorkerDailyTaskResourceModel>(
                url,
                "/api/recursos-humanos/obreros",
                $"/tareo/listar?taskDate={this.TaskDate}&sewerGroupId={this.SewerGroupId}",
                "bearer",
                MainViewModel.GetInstance().Token.Access_Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myWorkers = (List<WorkerDailyTaskResourceModel>)response.Result;
            this.Workers = new ObservableCollection<WorkerDailyTaskResourceModel>(myWorkers);
            this.IsRefreshing = false;
        }
        #endregion

        #region EditDailyTaskCommand
        public ICommand EditDailyTaskCommand => new RelayCommand<WorkerDailyTaskResourceModel>(this.EditDailyTask);
        private async void EditDailyTask(WorkerDailyTaskResourceModel obj)
        {
            MainViewModel.GetInstance().WorkerDailyTaskEditViewModel = new WorkerDailyTaskEditViewModel(obj);
            await App.Navigator.PushAsync(new WorkerDailyTaskEditPage());
        }
        #endregion

        private readonly ApiService apiService;
        public string TaskDate { get; set; }
        public Guid SewerGroupId { get; set; }
        public WorkerDailyTaskListViewModel(string _taskDate, Guid _sewerGroupId)
        {
            this.apiService = new ApiService();
            this.TaskDate = _taskDate;
            this.SewerGroupId = _sewerGroupId;
            this.IsRefreshing = false;
            this.IsEnabled = true;
            LoadWorkers();
        }
    }
}
