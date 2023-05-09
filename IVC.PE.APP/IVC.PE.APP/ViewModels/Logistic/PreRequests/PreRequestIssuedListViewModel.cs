using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Logistic.PreRequests;
using IVC.PE.BINDINGRESOURCES.Areas.Logistic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Logistic.PreRequests
{
   public class PreRequestIssuedListViewModel : BaseViewModel
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
            set
            {
                this.SetValue(ref this.selectedProject, value);
            }

        }
        #endregion

        #region BluePrintList
        private ObservableCollection<PreRequestAllListResourceModel> bpList;
        public ObservableCollection<PreRequestAllListResourceModel> BPList
        {
            get { return this.bpList; }
            set { this.SetValue(ref this.bpList, value); }
        }
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region LoadBluePrintsCommand
        public ICommand LoadBluePrintsCommand => new RelayCommand(this.LoadBluePrints);
        private async void LoadBluePrints()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<PreRequestAllListResourceModel>(
                url,
                "/api/logistica/pre-requerimientos",
                "/listar-filtrado-emitido?projectId=" + this.SelectedProject.Id
                + "&userId=" + MainViewModel.GetInstance().Token.UserId,
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

            var myBps = (List<PreRequestAllListResourceModel>)response.Result;
            this.BPList = new ObservableCollection<PreRequestAllListResourceModel>(myBps);

            MainViewModel.GetInstance().PreRequestIssuedListDetailViewModel = new PreRequestIssuedListDetailViewModel(this.BPList);
            await App.Navigator.PushAsync(new PreRequestListDetailPage());
        }

        #endregion

        private readonly ApiService apiService;

        public PreRequestIssuedListViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = false;
            LoadProjects();
        }
    }
}
