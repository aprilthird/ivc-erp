using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Logistic.PreRequests;
using IVC.PE.BINDINGRESOURCES.Areas.Logistic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Logistic.PreRequests
{
    public class PreRequestDetailCreateViewModel : BaseViewModel
    {
        //--------
        #region Front
        private ObservableCollection<SelectItem> fronts;
        public ObservableCollection<SelectItem> Fronts
        {
            get { return this.fronts; }
            set { this.SetValue(ref this.fronts, value); }
        }
        private async void LoadFronts()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/frentes-formula-app?projectFormulaId={this.SelectedProjectFormulaId}&projectId={this.ProjectId}");

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
            this.Fronts = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedFront
        private SelectItem selectedFront;
        public SelectItem SelectedFront
        {
            get { return this.selectedFront; }
            set
            {
                this.SetValue(ref this.selectedFront, value);
                

            }
        }
        #endregion

        //--------------
        #region Supplies
        private ObservableCollection<SelectItem> supplies;
        public ObservableCollection<SelectItem> Supplies
        {
            get { return this.supplies; }
            set { this.SetValue(ref this.supplies, value); }
        }
        private async void LoadSupplies()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/insumos-acero?" +
                $"&projectFormulaId={this.SelectedProjectFormulaId}&projectId={this.ProjectId}");

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
            this.Supplies = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }

        #endregion
        #region SelectedSupply
        private SelectItem selectedSupply;
        public SelectItem SelectedSupply
        {
            get { return this.selectedSupply; }
            set
            {
                this.SetValue(ref this.selectedSupply, value);
            }
        }
        #endregion
        //--------------

        //--
        private string observations;
        public string Observations
        {
            get { return this.observations; }
            set
            {
                this.SetValue(ref this.observations, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private string manualEntrySupply;
        public string ManualEntrySupply
        {
            get { return this.manualEntrySupply; }
            set
            {
                this.SetValue(ref this.manualEntrySupply, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private string manualEntryUnit;
        public string ManualEntryUnit
        {
            get { return this.manualEntryUnit; }
            set
            {
                this.SetValue(ref this.manualEntryUnit, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private bool isSupplyVisible;
        public bool IsSupplyVisible
        {
            get { return this.isSupplyVisible; }
            set
            {
                this.SetValue(ref this.isSupplyVisible, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private bool isManualVisible;
        public bool IsManualVisible
        {
            get { return this.isManualVisible; }
            set
            {
                this.SetValue(ref this.isManualVisible, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private bool supplyManual;
        public bool SupplyManual
        {
            get { return this.supplyManual; }
            set
            {
                this.SetValue(ref this.supplyManual, value);
                OnPropertyChanged();
            }
        }
        //--

        //--
        private float measure;
        public float Measure
        {
            get { return this.measure; }
            set
            {
                this.SetValue(ref this.measure, value);
                OnPropertyChanged();
            }
        }
        //--

        public Guid SelectedProjectFormulaId { get; set; }

        public Guid SelectedFamilyId { get; set; }

        public Guid PreRequestId { get; set; }

        public Guid ProjectId { get; set; }



        

        private readonly ApiService apiService;

        //--------------
        #region LoadBluePrintsCommand
        public ICommand ToSaveCommand => new RelayCommand(this.LoadBluePrints);
        private async void LoadBluePrints()
        {

            var toSave = new PreRequestDetailRegisterResourceModel
            {
                PreRequestId = this.PreRequestId,
                Measure = this.Measure,
                SupplyId = this.SelectedSupply.Id != null? this.SelectedSupply.Id : Guid.Empty,
                WorkFrontId = this.SelectedFront.Id,
                Observations = this.Observations,
                SupplyManual = this.SupplyManual,
                MeasurementUnitName = this.ManualEntryUnit,
                SupplyName = this.ManualEntrySupply
            };

            var url2 = Application.Current.Resources["UrlAPI"].ToString();
            var response2 = await this.apiService.PostAsync(
                url2,
                "/api/logistica/pre-requerimientos",
                "/detalle/registrar",
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

        }

        #endregion
        //--------------

        //--------------
        #region ChangeEnableCommand
        public ICommand ChangeEnableCommand => new RelayCommand(this.ChangeEnable);
        private void ChangeEnable()
        {
            if (this.IsSupplyVisible == true)
                this.IsSupplyVisible = false;
            else
                this.IsSupplyVisible = true;

            if (this.IsManualVisible == true)
            {
                this.IsManualVisible = false;
                this.SupplyManual = false;
            }
            else
            {
                this.IsManualVisible = true;
                this.SupplyManual = true;
            }


        }

        #endregion
        //--------------

        //--------------
        #region LoadBluePrintsCommand
        public ICommand ToSaveLoopCommand => new RelayCommand(this.ToSaveLoop);
        private async void ToSaveLoop()
        {

            var toSave = new PreRequestDetailRegisterResourceModel
            {
                PreRequestId = this.PreRequestId,
                Measure = this.Measure,
                SupplyId = this.SelectedSupply.Id != null ? this.SelectedSupply.Id : Guid.Empty,
                WorkFrontId = this.SelectedFront.Id,
                Observations = this.Observations,
                SupplyManual = this.SupplyManual,
                MeasurementUnitName = this.ManualEntryUnit,
                SupplyName = this.ManualEntrySupply
            };

            var url2 = Application.Current.Resources["UrlAPI"].ToString();
            var response2 = await this.apiService.PostAsync(
                url2,
                "/api/logistica/pre-requerimientos",
                "/detalle/registrar",
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

            MainViewModel.GetInstance().PreRequestDetailCreateViewModel = new PreRequestDetailCreateViewModel(this.SelectedProjectFormulaId, this.PreRequestId, this.ProjectId);
            await App.Navigator.PushAsync(new PreRequestDetailCreatePage());
        }

        #endregion
        //--------------
        public PreRequestDetailCreateViewModel(Guid projectFormulaId,Guid preRequestId, Guid projectId)
        {
            this.apiService = new ApiService();
            this.SelectedProjectFormulaId = projectFormulaId;
            this.PreRequestId = preRequestId;
            this.ProjectId = projectId;
            this.IsSupplyVisible = true;
            this.IsManualVisible = false;
            this.SupplyManual = false;
            LoadFronts();
            LoadSupplies();
        }

    }
}
