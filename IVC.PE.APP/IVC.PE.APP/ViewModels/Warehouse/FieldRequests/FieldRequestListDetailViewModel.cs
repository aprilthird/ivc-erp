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
    public class FieldRequestListDetailViewModel : BaseViewModel
    {
        #region BluePrintList
        private ObservableCollection<FieldRequestResourceModel> bpList;
        public ObservableCollection<FieldRequestResourceModel> BPList
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


        #region DetailListCommand
        public ICommand DetailListCommand => new RelayCommand<FieldRequestResourceModel>(this.DetailList);
        private async void DetailList(FieldRequestResourceModel obj)
        {
            MainViewModel.GetInstance().FieldRequestDetailListDetailViewModel = new FieldRequestDetailListDetailViewModel(obj.Id,obj.WorkFrontId.Value,obj.ProjectId);
            await App.Navigator.PushAsync(new FieldRequestDetailListDetailPage());
        }
        #endregion

        #region ItemCommand
        public ICommand ItemCommand => new RelayCommand<FieldRequestResourceModel>(this.Item);
        private async void Item(FieldRequestResourceModel obj)
        {
            MainViewModel.GetInstance().FieldRequestDetailCreateViewModel = new FieldRequestDetailCreateViewModel(obj.Id, obj.WorkFrontId.Value,obj.ProjectId);
            await App.Navigator.PushAsync(new FieldRequestDetailCreatePage());
        }
        #endregion

        #region EditCommand
        public ICommand EditCommand => new RelayCommand<FieldRequestResourceModel>(this.Edit);
        private async void Edit(FieldRequestResourceModel obj)
        {
            MainViewModel.GetInstance().FieldRequestEditViewModel = new FieldRequestEditViewModel(obj.Id);
            await App.Navigator.PushAsync(new FieldRequestEditPage());
        }

        #endregion
        //

        #region DeleteCommand
        public ICommand DeleteCommand => new RelayCommand<FieldRequestResourceModel>(this.Delete);
        private async void Delete(FieldRequestResourceModel obj)
        {
            var url2 = Application.Current.Resources["UrlAPI"].ToString();

            var toDelete = new FieldRequestResourceModel
            {
                Id = obj.Id,
                
            };
            var response2 = await this.apiService.PostAsync(
            url2,
            "/api/almacenes/pedidos-campo",
            $"/eliminar/{obj.Id}",
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

        #region EmitCommand
        public ICommand EmitCommand => new RelayCommand<FieldRequestResourceModel>(this.Emit);
        private async void Emit(FieldRequestResourceModel obj)
        {
            var url2 = Application.Current.Resources["UrlAPI"].ToString();

            var toEmit = new FieldRequestResourceModel
            {
                Id = obj.Id,
                ProjectId = obj.ProjectId
            };

            var response2 = await this.apiService.PutAsync(
                url2,
                "/api/almacenes/pedidos-campo",
                $"/emitir/{obj.Id}?projectId={obj.ProjectId}",
                toEmit,
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
        }
        #endregion


        #region FormulaListCommand
        public ICommand FormulaListCommand => new RelayCommand<FieldRequestResourceModel>(this.FormulaList);
        private async void FormulaList(FieldRequestResourceModel obj)
        {
            MainViewModel.GetInstance().FieldRequestCreateFormulaViewModel = new FieldRequestCreateFormulaViewModel(obj.Id,obj.ProjectId);
            await App.Navigator.PushAsync(new FieldRequestCreateFormulaPage());
        }

        #endregion
        //
        private readonly ApiService apiService;

        public FieldRequestListDetailViewModel(ObservableCollection<FieldRequestResourceModel> list)
        {
            this.apiService = new ApiService();
            this.BPList = list;
        }
    }
}
