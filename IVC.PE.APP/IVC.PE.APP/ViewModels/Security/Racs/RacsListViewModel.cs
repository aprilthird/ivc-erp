using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Security.Racs;
using IVC.PE.BINDINGRESOURCES.Areas.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Security.Racs
{
    public class RacsListViewModel : BaseViewModel
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
                "/proyectos");

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
            set { this.SetValue(ref this.selectedProject, value); }
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
            }
        }
        #endregion

        #region RacsList
        private ObservableCollection<RacsListResourceModel> racsList;
        public ObservableCollection<RacsListResourceModel> RacsList
        {
            get { return this.racsList; }
            set { this.SetValue(ref this.racsList, value); }
        }
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region LoadRacsCommand
        public ICommand LoadRacsCommand => new RelayCommand(this.LoadRacs);
        private async void LoadRacs()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<RacsListResourceModel>(
                url,
                "/api/seguridad/racs",
                "/listar?projectId=" + this.SelectedProject.Id + "&reportDate=" + this.SelectedDate.ToString("dd/MM/yyyy"),
                "bearer",
                MainViewModel.GetInstance().Token.Access_Token);

            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myRacs = (List<RacsListResourceModel>)response.Result;
            this.RacsList = new ObservableCollection<RacsListResourceModel>(myRacs);
        }
        public void UpdateRacs()
        {
            LoadRacs();
        }
        #endregion

        #region AddRacsCommand
        public ICommand AddRacsCommand => new RelayCommand(this.AddRacs);

        private async void AddRacs()
        {
            MainViewModel.GetInstance().RacsAddViewModel = new RacsAddViewModel(this);
            await App.Navigator.PushAsync(new RacsAddPage());
        }
        #endregion

        #region ViewRacsCommand
        public ICommand ViewRacsCommand => new RelayCommand<RacsListResourceModel>(this.ViewRacs);
        private async void ViewRacs(RacsListResourceModel obj)
        {
            MainViewModel.GetInstance().RacsViewModel = new RacsViewModel(obj.Id);
            await App.Navigator.PushAsync(new RacsPage());
        }
        #endregion

        #region LiftRacsCommand
        public ICommand LiftRacsCommand => new RelayCommand<RacsListResourceModel>(this.LiftRacs);
        private async void LiftRacs(RacsListResourceModel obj)
        {
            MainViewModel.GetInstance().RacsLiftViewModel = new RacsLiftViewModel(obj.Id, this);
            await App.Navigator.PushAsync(new RacsLiftPage());
        }
        #endregion

        #region PdfRacsCommand
        public ICommand PdfRacsCommand => new RelayCommand<RacsListResourceModel>(this.PdfRacs);
        private async void PdfRacs(RacsListResourceModel obj)
        {
            MainViewModel.GetInstance().RacsPdfViewModel = new RacsPdfViewModel(obj.Id);
            await App.Navigator.PushAsync(new RacsPdfPage());
        }
        #endregion

        #region ShareRacsCommand
        public ICommand ShareRacsCommand => new RelayCommand<RacsListResourceModel>(this.ShareRacs);
        private async void ShareRacs(RacsListResourceModel obj)
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            url = $"{url}/seguridad/racs/generar-pdf/{obj.Id}";
            await Share.RequestAsync(new ShareTextRequest
            {
                Title = "RACS",
                Text = obj.Code,
                Uri = url
            });            
        }
        #endregion

        private readonly ApiService apiService;
        public RacsListViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = false;
            this.SelectedDate = DateTime.Today;
            LoadProjects();
        }
    }
}
