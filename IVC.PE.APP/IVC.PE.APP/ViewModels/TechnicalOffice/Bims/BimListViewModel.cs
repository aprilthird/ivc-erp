using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.TechnicalOffice.Bims;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.Bims
{
    public class BimListViewModel : BaseViewModel
    {
        #region Formulas
        private ObservableCollection<SelectItem> formulas;
        public ObservableCollection<SelectItem> Formulas
        {
            get { return this.formulas; }
            set { this.SetValue(ref this.formulas, value); }
        }
        private async void LoadFormulas()
        {
            if (this.SelectedProject == null)
            {
                this.SelectedFormula = null;
                this.Formulas = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/formulas-por-proyecto?projectId={this.SelectedProject.Id}");

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
            this.Formulas = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedFormula
        private SelectItem selectedFormula;
        public SelectItem SelectedFormula
        {
            get { return this.selectedFormula; }
            set
            {
                this.SetValue(ref this.selectedFormula, value);
                LoadWorkFronts();
            }
        }
        #endregion


        //--------------
        #region WorkFronts
        private ObservableCollection<SelectItem> workFronts;
        public ObservableCollection<SelectItem> WorkFronts
        {
            get { return this.workFronts; }
            set { this.SetValue(ref this.workFronts, value); }
        }
        private async void LoadWorkFronts()
        {
            if (this.SelectedFormula == null)
            {
                this.SelectedWorkFront = null;
                this.WorkFronts = new ObservableCollection<SelectItem>();
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/frentes-formula?projectFormulaId={this.SelectedFormula.Id}");

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
            this.WorkFronts = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedWorkFront
        private SelectItem selectedWorkFront;
        public SelectItem SelectedWorkFront
        {
            get { return this.selectedWorkFront; }
            set
            {
                this.SetValue(ref this.selectedWorkFront, value);

            }
        }
        #endregion
        //--------------

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
                LoadFormulas();

            }

        }
        #endregion
        //--------------
        private string name;

        public string Name
        {
            get { return this.name; }
            set
            {
                this.SetValue(ref this.name, value);
                OnPropertyChanged();
            }

        }
        //--
        #region BimList
        private ObservableCollection<BimResourceModel> bpList;
        public ObservableCollection<BimResourceModel> BPList
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

        #region LoadBimsCommand
        public ICommand LoadBimsCommand => new RelayCommand(this.LoadBims);
        private async void LoadBims()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<BimResourceModel>(
                url,
                "/api/oficina-tecnica/bim",
                "/listar?projectId=" + this.SelectedProject.Id +
                (this.SelectedWorkFront != null ? ("&workFrontId=" + this.SelectedWorkFront.Id) : string.Empty) +
                (this.SelectedFormula != null ? ("&formulaId=" + this.SelectedFormula.Id) : string.Empty) +
                (this.Name != null ? ("&str=" + this.Name) : string.Empty),
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

            var myBps = (List<BimResourceModel>)response.Result;
            this.BPList = new ObservableCollection<BimResourceModel>(myBps);

            MainViewModel.GetInstance().BimListDetailViewModel = new BimListDetailViewModel(this.BPList);
            await App.Navigator.PushAsync(new BimListDetailPage());
        }

        #endregion
        private readonly ApiService apiService;
        public BimListViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = false;
            LoadProjects();
        }
    }
}
