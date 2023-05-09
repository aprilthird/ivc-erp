using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Warehouse.FieldRequests
{
  public  class FieldRequestCreateFormulaViewModel : BaseViewModel
    {

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


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/formulas-por-proyecto?projectId={this.SelectedProjectId}");

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

        public Guid Id { get; set; }

        public Guid SelectedProjectId { get; set; }

        private readonly ApiService apiService;

        //--------------
        #region LoadBluePrintsCommand
        public ICommand ToSaveCommand => new RelayCommand(this.LoadBluePrints);
        private async void LoadBluePrints()
        {

            var toSave = new FieldRequestFatherFormulaRegisterResourceModel
            {


                Id = this.Id,
                ProjectFormulaId = this.SelectedFormula.Id,

            };

            var url2 = Application.Current.Resources["UrlAPI"].ToString();
            var response2 = await this.apiService.PostAsync(
                url2,
                "/api/almacenes/pedidos-campo",
                "/parte-padre-formula/registrar",
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

        public FieldRequestCreateFormulaViewModel(Guid id, Guid projectId)
        {
            this.apiService = new ApiService();
            this.Id = id;
            this.SelectedProjectId = projectId;
            LoadFormulas();
            //this.FormulaId = formulaId;
            //this.FamilyId = familyId;
            //this.ProjectId = projectId;
            

        }
    }
}
