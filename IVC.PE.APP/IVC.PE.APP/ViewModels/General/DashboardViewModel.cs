using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.BINDINGRESOURCES.Areas.General.Dashboard;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.General
{
    public class DashboardViewModel : BaseViewModel
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
                this.GetWorkersByWeek();
                this.GetHoursByWeek();
                this.GetCostsByWeek();
            }
        }
        #endregion

        #region WorkersByWeek
        private ChartEntry[] workersByWeek;
        public ChartEntry[] WorkersByWeek
        {
            get { return this.workersByWeek; }
            set { this.SetValue(ref this.workersByWeek, value); }
        }
        private async void GetWorkersByWeek()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<DashboardChartResourceModel>(
                url,
                "/api/general/dashboard",
                $"/obreros-semana/{this.SelectedProject.Id}",
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

            var myWorkers = (List<DashboardChartResourceModel>)response.Result;
            this.WorkersByWeek = myWorkers.Select(x => new ChartEntry(float.Parse(x.Value))
            {
                Label = x.Label,
                ValueLabel = x.Value,
                Color = SKColor.Parse("#2c3e50")
            }).ToArray();
            this.LoadWorkersChart();
        }
        #endregion

        #region HoursByWeek
        private ChartEntry[] hoursByWeek;
        public ChartEntry[] HoursByWeek
        {
            get { return this.hoursByWeek; }
            set { this.SetValue(ref this.hoursByWeek, value); }
        }
        private async void GetHoursByWeek()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<DashboardChartResourceModel>(
                url,
                "/api/general/dashboard",
                $"/horas-semana/{this.SelectedProject.Id}",
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

            var myHours = (List<DashboardChartResourceModel>)response.Result;
            this.HoursByWeek = myHours.Select(x => new ChartEntry(float.Parse(x.Value))
            {
                Label = x.Label,
                ValueLabel = x.Value,
                Color = SKColor.Parse("#2c3e50")
            }).ToArray();
            this.LoadHoursChart();
        }
        #endregion

        #region HourssByWeek
        private ChartEntry[] costsByWeek;
        public ChartEntry[] CostsByWeek
        {
            get { return this.costsByWeek; }
            set { this.SetValue(ref this.costsByWeek, value); }
        }
        private async void GetCostsByWeek()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<DashboardChartResourceModel>(
                url,
                "/api/general/dashboard",
                $"/costos-semana/{this.SelectedProject.Id}",
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

            var myHours = (List<DashboardChartResourceModel>)response.Result;
            this.CostsByWeek = myHours.Select(x => new ChartEntry(float.Parse(x.Value))
            {
                Label = x.Label,
                ValueLabel = x.Value,
                Color = SKColor.Parse("#2c3e50")
            }).ToArray();
            this.LoadCostsChart();
        }
        #endregion

        private Chart workersByWeekChart;
        public Chart WorkersByWeekChart
        {
            get { return this.workersByWeekChart; }
            set { this.SetValue(ref this.workersByWeekChart, value); }
        }
        private Chart hoursByWeekChart;
        public Chart HoursByWeekChart
        {
            get { return this.hoursByWeekChart; }
            set { this.SetValue(ref this.hoursByWeekChart, value); }
        }
        private Chart costsByWeekChart;
        public Chart CostsByWeekChart
        {
            get { return this.costsByWeekChart; }
            set { this.SetValue(ref this.costsByWeekChart, value); }
        }

        private readonly ApiService apiService;

        public DashboardViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProjects();
        }

        private void LoadWorkersChart()
        {
            this.WorkersByWeekChart = new LineChart
            {
                Entries = this.WorkersByWeek,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 30
            };
        }

        private void LoadHoursChart()
        {
            this.HoursByWeekChart = new LineChart
            {
                Entries = this.HoursByWeek,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 30
            };
        }

        private void LoadCostsChart()
        {
            this.CostsByWeekChart = new LineChart
            {
                Entries = this.CostsByWeek,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 30
            };
        }
    }
}
