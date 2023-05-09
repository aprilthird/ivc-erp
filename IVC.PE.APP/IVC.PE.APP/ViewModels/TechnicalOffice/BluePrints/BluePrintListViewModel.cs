using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.TechnicalOffice.BluePrints;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.BluePrints
{
    public class BluePrintListViewModel : BaseViewModel
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
                LoadSpecs();
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
                $"/frentes-formula-app?projectFormulaId={this.SelectedFormula.Id}&projectId={this.selectedProject.Id}");

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
        #region Spec
        private ObservableCollection<SelectItem> specs;
        public ObservableCollection<SelectItem> Specs
        {
            get { return this.specs; }
            set { this.SetValue(ref this.specs, value); }
        }
        private async void LoadSpecs()
        {
            if (this.SelectedFormula == null && this.SelectedProject == null)
            {
                this.SelectedSpec = null;
                this.Specs = new ObservableCollection<SelectItem>();
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/especialidades-formula?projectFormulaId={this.SelectedFormula.Id}&projectId={this.SelectedProject.Id}");

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
            this.Specs = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedSpec
        private SelectItem selectedSpec;
        public SelectItem SelectedSpec
        {
            get { return this.selectedSpec; }
            set
            {
                this.SetValue(ref this.selectedSpec, value);
            }
        }
        #endregion
        //--------------
        #region BudgetTittles
        private ObservableCollection<SelectItem> budgetTittles;
        public ObservableCollection<SelectItem> BudgetTittles
        {
            get { return this.budgetTittles; }
            set { this.SetValue(ref this.budgetTittles, value); }
        }
        private async void LoadBudgets()
        {
            if (this.SelectedProject == null)
            {
                this.SelectedBudget = null;
                this.BudgetTittles = new ObservableCollection<SelectItem>();
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/titulos-de-presupuesto-area-tecnica?projectId={this.SelectedProject.Id}");

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
            this.BudgetTittles = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedBudget
        private SelectItem selectedBudget;
        public SelectItem SelectedBudget
        {
            get { return this.selectedBudget; }
            set
            {
                this.SetValue(ref this.selectedBudget, value);
                
            }
        }
        #endregion
        //--------------
        #region Versions
        private ObservableCollection<SelectItem> versions;
        public ObservableCollection<SelectItem> Versions
        {
            get { return this.versions; }
            set { this.SetValue(ref this.versions, value); }
        }
        private async void LoadVersions()
        {
            if (this.SelectedProject == null)
            {
                this.SelectedVersion = null;
                this.Versions = new ObservableCollection<SelectItem>();
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/versiones?projectId={this.SelectedProject.Id}");

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
            this.Versions = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedVersion
        private SelectItem selectedVersion;
        public SelectItem SelectedVersion
        {
            get { return this.selectedVersion; }
            set
            {
                this.SetValue(ref this.selectedVersion, value);

            }
        }
        #endregion
        //--------------
        private string name;

        public string Name
        {
            get { return this.name; }
            set { this.SetValue(ref this.name, value);
                OnPropertyChanged();
            }

        }
        //--
        #region Projects
        private ObservableCollection<SelectItem> projects;
        public ObservableCollection<SelectItem> Projects
        {
            get { return this.projects; }
            set { this.SetValue(ref this.projects, value);}
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
            set { this.SetValue(ref this.selectedProject, value);
                LoadFormulas();
                LoadBudgets();
                LoadVersions();
            }

        }
        #endregion

        #region BluePrintList
        private ObservableCollection<BluePrintListResourceModel> bpList;
            public ObservableCollection<BluePrintListResourceModel> BPList
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
                var response = await this.apiService.GetListAsync<BluePrintListResourceModel>(
                    url,
                    "/api/oficina-tecnica/planos",
                    "/listar?projectId=" + this.SelectedProject.Id+
                    (this.SelectedBudget!=null?("&budgetId="+this.SelectedBudget.Id):string.Empty)+ 
                    (this.SelectedWorkFront!=null?("&workFrontId="+this.SelectedWorkFront.Id) : string.Empty)+
                    (this.SelectedFormula!=null?("&formulaId="+this.SelectedFormula.Id):string.Empty) + 
                    (this.SelectedSpec!=null?("&specId="+this.SelectedSpec.Id):string.Empty)+
                    (this.SelectedVersion!=null?("&versionId="+this.SelectedVersion.Id):string.Empty)+
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

                var myBps = (List<BluePrintListResourceModel>)response.Result;
                this.BPList = new ObservableCollection<BluePrintListResourceModel>(myBps);

                MainViewModel.GetInstance().BluePrintListDetailViewModel = new BluePrintListDetailViewModel(this.BPList);
                await App.Navigator.PushAsync(new BluePrintListDetailPage());
            }

        #endregion




        private readonly ApiService apiService;
        public BluePrintListViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = false;
            LoadProjects();
        }
    }


}
