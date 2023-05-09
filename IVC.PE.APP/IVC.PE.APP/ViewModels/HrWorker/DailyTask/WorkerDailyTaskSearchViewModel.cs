using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.HrWorker.DailyTask;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.HrWorker.DailyTask
{
    public class WorkerDailyTaskSearchViewModel : BaseViewModel
    {
        #region Projects
        private ObservableCollection<SelectItem> projects;
        public ObservableCollection<SelectItem> Projects
        {
            get { return this.projects; }
            set { this.SetValue(ref this.projects, value); }
        }
        private async void LoadProjects()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                "/proyectos-usuario/" + MainViewModel.GetInstance().Token.UserId);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myProjects = (List<SelectItem>)response.Result;
            this.Projects = new ObservableCollection<SelectItem>(myProjects);
            this.SelectedProject = myProjects.FirstOrDefault();
        }
        #endregion
        #region SelectedProject
        private SelectItem selectedProject;
        public SelectItem SelectedProject
        {
            get { return this.selectedProject; }
            set { 
                this.SetValue(ref this.selectedProject, value);
                LoadWorkFrontHeads();
            }
        }
        #endregion

        #region WorkFrontHeads
        private ObservableCollection<SelectItem> workFrontHeads;
        public ObservableCollection<SelectItem> WorkFrontHeads
        {
            get { return this.workFrontHeads; }
            set { this.SetValue(ref this.workFrontHeads, value); }
        }
        private async void LoadWorkFrontHeads()
        {
            if (this.SelectedProject == null)
            {
                this.SelectedWorkFrontHead = null;
                this.WorkFrontHeads = new ObservableCollection<SelectItem>();
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/jefes-de-frente-activos?dateStr={this.SelectedDate.ToString("dd/MM/yyyy")}&projectId={this.SelectedProject.Id}");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myWorkFrontHeads = (List<SelectItem>)response.Result;
            this.WorkFrontHeads = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedWorkFrontHead
        private SelectItem selectedWorkFrontHead;
        public SelectItem SelectedWorkFrontHead
        {
            get { return this.selectedWorkFrontHead; }
            set
            {
                this.SetValue(ref this.selectedWorkFrontHead, value);
                LoadSewerGroups();
            }
        }
        #endregion

        #region SewerGroups
        private ObservableCollection<SelectItem> sewerGroups;
        public ObservableCollection<SelectItem> SewerGroups
        {
            get { return this.sewerGroups; }
            set { this.SetValue(ref this.sewerGroups, value); }
        }
        private async void LoadSewerGroups()
        {
            if (this.SelectedProject == null || this.SelectedWorkFrontHead == null)
            {
                this.SelectedSewerGroup = null;
                this.SewerGroups = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/cuadrillas?reportDate={this.SelectedDate.ToString("dd/MM/yyyy")}&projectId={this.SelectedProject.Id}&workFrontHeadId={this.SelectedWorkFrontHead.Id}");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var mySewerGroups = (List<SelectItem>)response.Result;
            this.SewerGroups = new ObservableCollection<SelectItem>(mySewerGroups);
        }
        #endregion
        #region SelectedSewerGroup
        private SelectItem selectedSewerGroup;
        public SelectItem SelectedSewerGroup
        {
            get { return this.selectedSewerGroup; }
            set { this.SetValue(ref this.selectedSewerGroup, value); }
        }
        #endregion

        #region SelectedDate
        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return this.selectedDate; }
            set
            {
                this.SetValue(ref this.selectedDate, value);
                LoadWorkFrontHeads();
            }
        }
        #endregion

        #region IsRefreshing
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region LoadAttendanceCommand
        public ICommand LoadAttendanceCommand => new RelayCommand(this.LoadAttendance);
        private async void LoadAttendance()
        {
            MainViewModel.GetInstance().WorkerDailyTaskListViewModel = new WorkerDailyTaskListViewModel(this.SelectedDate.ToString("dd/MM/yyyy"), this.SelectedSewerGroup.Id);
            await App.Navigator.PushAsync(new WorkerDailyTaskListPage());
        }
        #endregion

        private readonly ApiService apiService;
        public WorkerDailyTaskSearchViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = false;
            this.SelectedDate = DateTime.Today;
            LoadProjects();
        }
    }
}
