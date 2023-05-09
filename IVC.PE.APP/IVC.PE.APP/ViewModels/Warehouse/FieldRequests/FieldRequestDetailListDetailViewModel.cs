using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Warehouse.FieldRequests;
using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Warehouse.FieldRequests
{
    public class FieldRequestDetailListDetailViewModel : BaseViewModel
    {
        #region BluePrintList
        private ObservableCollection<FieldRequestFoldingResourceModel> bpList;
        public ObservableCollection<FieldRequestFoldingResourceModel> BPList
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
        //

        #region Load

        private async void LoadDetails()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<FieldRequestFoldingResourceModel>(
                url,
                "/api/almacenes/pedidos-campo",
                "/detalles/listar?id=" + this.FieldRequestId,
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

            var myBps = (List<FieldRequestFoldingResourceModel>)response.Result;
            this.BPList = new ObservableCollection<FieldRequestFoldingResourceModel>(myBps);

        }

        #endregion
        //
        public Guid FieldRequestId { get; set; }

        public Guid WorkFrontId { get; set; }
        public Guid SupplyFamilyId{ get; set; }
        public Guid ProjectId { get; set; }

    //public Guid FormulaId { get; set; }
    //public Guid FamilyId { get; set; }
    //public Guid ProjectId { get; set; }


    private readonly ApiService apiService;

        #region DetailEditCommand
        public ICommand DetailEditCommand => new RelayCommand<FieldRequestFoldingResourceModel>(this.Detail);
        private async void Detail(FieldRequestFoldingResourceModel obj)
        {
            MainViewModel.GetInstance().FieldRequestDetailEditViewModel = new FieldRequestDetailEditViewModel(obj.Id, this.FieldRequestId, this.WorkFrontId, this.SupplyFamilyId, this.ProjectId);
            await App.Navigator.PushAsync(new FieldRequestDetailEditPage());
        }
        #endregion
        //



        #region DeleteCommand
        public ICommand DeleteCommand => new RelayCommand<FieldRequestFoldingResourceModel>(this.Delete);
        private async void Delete(FieldRequestFoldingResourceModel obj)
        {
            var url2 = Application.Current.Resources["UrlAPI"].ToString();

            var toDelete = new FieldRequestFoldingResourceModel
            {
                Id = obj.Id,
            };
            var response2 = await this.apiService.PostAsync(
            url2,
            "/api/almacenes/pedidos-campo",
            $"/detalles/eliminar/{obj.Id}",
            toDelete,
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
                    ConstantHelpers.ResponseMessages.SUCCESS_REMOVED,
                    ConstantHelpers.ResponseMessages.OK
                    );

            await App.Navigator.PopAsync();
        }
        #endregion

        //
        public FieldRequestDetailListDetailViewModel(Guid fieldId, Guid WorkFrontId, Guid ProjectId)
        {
            this.apiService = new ApiService();
            this.FieldRequestId = fieldId;
            this.WorkFrontId = this.WorkFrontId;
            this.ProjectId = this.ProjectId;
            LoadDetails();

        }
    }
}
