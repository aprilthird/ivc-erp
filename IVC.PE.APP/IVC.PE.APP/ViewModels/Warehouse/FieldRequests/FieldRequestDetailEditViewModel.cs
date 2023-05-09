using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
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
    public class FieldRequestDetailEditViewModel : BaseViewModel
    {
        public Guid Id { get; set; }
        public Guid FieldId { get; set; }

        public Guid FrontId { get; set; }

        public Guid FamilyId { get; set; }

        public Guid ProjectId { get; set; }

        //--
        #region Budgets
        private ObservableCollection<SelectItem> budgets;
        public ObservableCollection<SelectItem> Budgets
        {
            get { return this.budgets; }
            set { this.SetValue(ref this.budgets, value); }
        }
        private async void LoadBudgets()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                "/insumos-de-meta-pedidos-campo-app?" +
                $"fieldRequestId={this.FieldId}&workFrontId={this.FrontId}&supplyFamilyId={this.FamilyId}&projectId={this.ProjectId}");

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
            this.Budgets = new ObservableCollection<SelectItem>(myProjects);
            this.SelectedBudget = myProjects.FirstOrDefault();

        }
        #endregion

        #region SelectedProject
        private SelectItem selectedBudget;
        public SelectItem SelectedBudget
        {
            get { return this.selectedBudget; }
            set
            {
                this.SetValue(ref this.selectedBudget, value);
                this.LoadPhases();
                this.LoadMethods();
            }

        }
        #endregion

        //-------------

        //--------------

        #region Phases
        private ObservableCollection<SelectItem> phases;
        public ObservableCollection<SelectItem> Phases
        {
            get { return this.phases; }
            set { this.SetValue(ref this.phases, value); }
        }
        private async void LoadPhases()
        {
            if (this.SelectedBudget == null)
            {
                this.SelectedPhase = null;
                this.Phases = new ObservableCollection<SelectItem>();
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/fases-por-insumo-meta?goalBudgetInputId={this.SelectedBudget.Id}");

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
            this.Phases = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedPhase
        private SelectItem selectedPhase;
        public SelectItem SelectedPhase
        {
            get { return this.selectedPhase; }
            set
            {
                this.SetValue(ref this.selectedPhase, value);
            }
        }
        #endregion
        //--

        //--
        private string quantity;
        public string Quantity
        {
            get { return this.quantity; }
            set
            {
                this.SetValue(ref this.quantity, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private string red;
        public string Red
        {
            get { return this.red; }
            set
            {
                this.SetValue(ref this.red, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private string blue;
        public string Blue
        {
            get { return this.blue; }
            set
            {
                this.SetValue(ref this.blue, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private string black;
        public string Black
        {
            get { return this.black; }
            set
            {
                this.SetValue(ref this.black, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private string abb;
        public string Abb
        {
            get { return this.abb; }
            set
            {
                this.SetValue(ref this.abb, value);
                OnPropertyChanged();
            }
        }
        //--

        private async void LoadMethods()
        {
            if (this.SelectedBudget == null)
            {
                this.Red = null;
                this.Blue = null;
                this.Black = null;
                this.Abb = null;

                return;
            }

            else
            {

                var url = Application.Current.Resources["UrlAPI"].ToString();
                var response = await this.apiService.GetListAsync<string>(
                    url,
                    "/api/almacenes/pedidos-campo",
                    $"/insumo-meta/{this.SelectedBudget.Id}",
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

                var myWorkFrontHeads = (List<string>)response.Result;
                this.Red = "límite:" + myWorkFrontHeads[0];
                this.Blue = "techo:" + myWorkFrontHeads[2];
                this.Black = "stock: " + myWorkFrontHeads[1];
                this.Abb = myWorkFrontHeads[3];

                return;
            }
        }

        private readonly ApiService apiService;

        //--------------
        #region LoadBluePrintsCommand
        public ICommand ToSaveCommand => new RelayCommand(this.LoadBluePrints);
        private async void LoadBluePrints()
        {

            var toSave = new FieldRequestFoldingRegisterResourceModel
            {

                Id = this.Id,

                FieldRequestId = this.FieldId,

                GoalBudgetInputId = this.SelectedBudget.Id,

                ProjectPhaseId = this.SelectedPhase.Id,

                Quantity = this.quantity,
            };

            var url2 = Application.Current.Resources["UrlAPI"].ToString();
            var response2 = await this.apiService.PutAsync(
                url2,
                "/api/almacenes/pedidos-campo",
                $"/detalle/editar/{this.Id}",
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

        public FieldRequestDetailEditViewModel(Guid id,Guid fieldRequestId, Guid workFrontId, Guid supplyFamilyId, Guid projectId)
        {
            this.Id = Id;
            this.apiService = new ApiService();
            this.FieldId = fieldRequestId;
            this.FrontId = workFrontId;
            this.FamilyId = supplyFamilyId;
            this.ProjectId = projectId;
            LoadBudgets();
        }
    }
}
