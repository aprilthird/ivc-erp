using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Warehouse.Techos;
using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Warehouse.Techos
{
   public class TechoListViewModel : BaseViewModel
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


        #region Groups
        private ObservableCollection<SelectItem> groups;
        public ObservableCollection<SelectItem> Groups
        {
            get { return this.groups; }
            set { this.SetValue(ref this.groups, value); }
        }
        private async void LoadGroups()
        {
            if (this.SelectedFamily == null)
            {
                this.SelectedGroup = null;
                this.Groups = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/grupos-de-insumos-familia?familyId={this.SelectedFamily.Id}");

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
            this.Groups = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedGroup
        private SelectItem selectedGroup;
        public SelectItem SelectedGroup
        {
            get { return this.selectedGroup; }
            set
            {
                this.SetValue(ref this.selectedGroup, value);

            }
        }
        #endregion

        //--------------

        #region Families
        private ObservableCollection<SelectItem> families;
        public ObservableCollection<SelectItem> Families
        {
            get { return this.families; }
            set { this.SetValue(ref this.families, value); }
        }
        private async void LoadFamilies()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/familias-de-insumos");

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
            this.Families = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedFamily
        private SelectItem selectedFamily;
        public SelectItem SelectedFamily
        {
            get { return this.selectedFamily; }
            set
            {
                this.SetValue(ref this.selectedFamily, value);
                LoadGroups();
            }
        }
        #endregion
        //--------------

        //--
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
                LoadBudgets();

            }

        }
        #endregion

        #region TechoList
        private ObservableCollection<TechoResourceModel> bpList;
        public ObservableCollection<TechoResourceModel> BPList
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
            var response = await this.apiService.GetListAsync<TechoResourceModel>(
                url,
                "/api/almacenes/existencias-techos",
                "/listar?projectId=" + this.SelectedProject.Id +
                (this.SelectedBudget != null ? ("&budgetTitleId=" + this.SelectedBudget.Id) : string.Empty) +
                (this.SelectedWorkFront != null ? ("&workFrontId=" + this.SelectedWorkFront.Id) : string.Empty) +
                (this.SelectedFormula != null ? ("&projectFormulaId=" + this.SelectedFormula.Id) : string.Empty) +
                (this.SelectedGroup != null ? ("&supplyGroupId=" + this.SelectedGroup.Id) : string.Empty)+
                (this.SelectedFamily != null ? ("&supplyFamilyId=" + this.SelectedFamily.Id) : string.Empty),
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

            var myBps = (List<TechoResourceModel>)response.Result;
            this.BPList = new ObservableCollection<TechoResourceModel>(myBps);

            MainViewModel.GetInstance().TechoListDetailViewModel = new TechoListDetailViewModel(this.BPList);
            await App.Navigator.PushAsync(new TechoListDetailPage());
        }

        #endregion

        private readonly ApiService apiService;
        public TechoListViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = false;
            LoadProjects();
            LoadFamilies();
        }
    }
}
