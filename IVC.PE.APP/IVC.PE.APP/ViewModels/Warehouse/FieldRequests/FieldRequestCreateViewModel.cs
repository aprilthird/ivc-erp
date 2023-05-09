using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Warehouse.FieldRequests;
using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Warehouse.FieldRequests
{
    public class FieldRequestCreateViewModel : BaseViewModel
    {
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
                LoadWorkFronts();
                LoadSewers();
            }

        }
        #endregion

        //-------------

        //--
        #region Sewers
        private ObservableCollection<SelectItem> sewers;
        public ObservableCollection<SelectItem> Sewers
        {
            get { return this.sewers; }
            set { this.SetValue(ref this.sewers, value); }
        }
        private async void LoadSewers()
        {
            if (this.SelectedWorkFront == null && this.SelectedProject == null)
            {
                this.SelectedSewerGroup = null;
                this.Sewers = new ObservableCollection<SelectItem>();
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/cuadrillas-frente?projectId={this.SelectedProject.Id}" +
                (this.SelectedWorkFront != null ? ("&workFrontId=" + this.SelectedWorkFront.Id) : string.Empty));

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
            this.Sewers = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedSewerGroup
        private SelectItem selectedSewerGroup;
        public SelectItem SelectedSewerGroup
        {
            get { return this.selectedSewerGroup; }
            set
            {
                this.SetValue(ref this.selectedSewerGroup, value);

            }
        }
        #endregion
        //--------



        //--------------

        //--------------
        #region LoadBluePrintsCommand
        public ICommand ToSaveCommand => new RelayCommand(this.LoadBluePrints);
        private async void LoadBluePrints()
        {

            var toSave = new FieldRequestFatherRegisterResourceModel
            {



                BudgetTitleId = this.SelectedBudget.Id,
                IssuedUserId = MainViewModel.GetInstance().Token.UserId,
                //ProjectFormulaId = this.SelectedFormula.Id,
                WorkFrontId = this.SelectedWorkFront.Id,
                SewerGroupId = this.SelectedSewerGroup.Id,
                Observation = this.Ob,
                WorkOrder = this.Ot
            };

            var url2 = Application.Current.Resources["UrlAPI"].ToString();
            var response2 = await this.apiService.PostAsync(
                url2,
                "/api/almacenes/pedidos-campo",
                "/parte-padre/registrar",
                toSave,
                "bearer",
                MainViewModel.GetInstance().Token.Access_Token
                );


            if (!response2.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    ConstantHelpers.ResponseMessages.FAIL,
                    response2.Message,
                    ConstantHelpers.ResponseMessages.OK
                    );
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                    ConstantHelpers.ResponseMessages.SUCCESS,
                    ConstantHelpers.ResponseMessages.SUCCESS_ADDED,
                    ConstantHelpers.ResponseMessages.OK
                    );
            await App.Navigator.PopAsync();

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<FieldRequestResourceModel>(
                url,
                "/api/almacenes/pedidos-campo",
                "/listar?userId=" + MainViewModel.GetInstance().Token.UserId,
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

            var myBps = (List<FieldRequestResourceModel>)response.Result;
            var newF = new ObservableCollection<FieldRequestResourceModel>(myBps);

            MainViewModel.GetInstance().FieldRequestDetailCreateViewModel = new FieldRequestDetailCreateViewModel(newF.LastOrDefault().Id,this.SelectedWorkFront.Id,this.SelectedProject.Id);
            await App.Navigator.PushAsync(new FieldRequestDetailCreatePage());
        }

        #endregion
        //--------------

        //--------------


        //--
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
            }
        }
        #endregion
        //--
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
        //--------
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
                LoadSewers();
            }
        }
        #endregion
        //--------------

        //--
        private string ot;
        public string Ot
        {
            get { return this.ot; }
            set
            {
                this.SetValue(ref this.ot, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private string ob;
        public string Ob
        {
            get { return this.ob; }
            set
            {
                this.SetValue(ref this.ob, value);
                OnPropertyChanged();
            }
        }
        //--

        private readonly ApiService apiService;

        public FieldRequestCreateViewModel()
        {
            this.apiService = new ApiService();
            LoadProjects();
            LoadFamilies();
            this.SelectedDate = DateTime.UtcNow;
        }
    }
}
