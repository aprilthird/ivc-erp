using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
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
   public class PreRequestEditViewModel : BaseViewModel
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
            }

        }
        #endregion

        //-------------

        #region Types
        private ObservableCollection<SelectItemInt> types;
        public ObservableCollection<SelectItemInt> Types
        {
            get { return this.types; }
            set { this.SetValue(ref this.types, value); }
        }
        private async void LoadTypes()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItemInt>(
                url,
                "/select",
                "/tipo-generacion-pre-reque");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myProjects = (List<SelectItemInt>)response.Result;
            this.Types = new ObservableCollection<SelectItemInt>(myProjects);
            this.SelectedType = myProjects.FirstOrDefault();
        }
        #endregion

        #region SelectedProject
        private SelectItemInt selectedType;
        public SelectItemInt SelectedType
        {
            get { return this.selectedType; }
            set
            {
                this.SetValue(ref this.selectedType, value);
            }

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
            }
        }
        #endregion
        //--------------
        #region LoadBluePrintsCommand
        public ICommand ToSaveCommand => new RelayCommand(this.LoadBluePrints);
        private async void LoadBluePrints()
        {

            var toSave = new PreRequestEditResourceModel
            {




                BudgetTitleId = this.SelectedBudget.Id,
                RequestType = this.SelectedType.Id,
                IssuedUserId = MainViewModel.GetInstance().Token.UserId,
                SupplyFamilyId = this.SelectedFamily.Id,
                ProjectFormulaId = this.SelectedFormula.Id

            };

            var url2 = Application.Current.Resources["UrlAPI"].ToString();
            var response2 = await this.apiService.PutAsync(
                url2,
                "/api/logistica/pre-requerimientos",
                $"/editar/{this.PreRequestId}",
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
                    ConstantHelpers.ResponseMessages.SUCCESS_EDITED,
                    ConstantHelpers.ResponseMessages.OK
                    );
            await App.Navigator.PopAsync();

            //MainViewModel.GetInstance().EquipmentMachPartFoldingVariableViewModel = new EquipmentMachPartFoldingVariableViewModel(this.qrCode, this.SelectedUser, this.SelectedOperator, this.SelectedSewerGroup, this.PartNumber);
            //await App.Navigator.PushAsync(new EquipmentMachPartFoldingVariablePage());
        }

        #endregion
        //--------------
        public Guid PreRequestId { get; set; }

        //--

        private readonly ApiService apiService;

        public PreRequestEditViewModel(Guid preId)
        {
            this.apiService = new ApiService();
            LoadProjects();
            LoadFamilies();
            LoadTypes();
            this.SelectedDate = DateTime.UtcNow;
            this.PreRequestId = preId;

        }
    }
}
