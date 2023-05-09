using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Logistic.PreRequests;
using IVC.PE.BINDINGRESOURCES.Areas.Logistic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Logistic.PreRequests
{
   public class PreRequestListDetailViewModel : BaseViewModel
    {
        #region BluePrintList
        private ObservableCollection<PreRequestAllListResourceModel> bpList;
        public ObservableCollection<PreRequestAllListResourceModel> BPList
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
        #region DetailListCommand
        public ICommand DetailListCommand => new RelayCommand<PreRequestAllListResourceModel>(this.DetailList);
        private async void DetailList(PreRequestAllListResourceModel obj)
        {
            MainViewModel.GetInstance().PreRequestDetailListDetailViewModel = new PreRequestDetailListDetailViewModel(obj.Id,obj.ProjectFormulaId,obj.ProjectId);
            await App.Navigator.PushAsync(new PreRequestDetailListDetailPage());
        }

        #endregion
        //


        //
        #region DetailRegisterCommand
        public ICommand DetailRegisterCommand => new RelayCommand<PreRequestAllListResourceModel>(this.Detail);
        private async void Detail(PreRequestAllListResourceModel obj)
        {
            MainViewModel.GetInstance().PreRequestDetailCreateViewModel = new PreRequestDetailCreateViewModel(obj.ProjectFormulaId,obj.Id,obj.ProjectId);
            await App.Navigator.PushAsync(new PreRequestDetailCreatePage());
        }
        #endregion

        //

        #region EditCommand
        public ICommand EditCommand => new RelayCommand<PreRequestAllListResourceModel>(this.Edit);
        private async void Edit(PreRequestAllListResourceModel obj)
        {
            MainViewModel.GetInstance().PreRequestEditViewModel = new PreRequestEditViewModel(obj.Id);
            await App.Navigator.PushAsync(new PreRequestEditPage());
        }
        #endregion

        //

        #region EmitCommand
        public ICommand EmitCommand => new RelayCommand<PreRequestAllListResourceModel>(this.Emit);
        private async void Emit(PreRequestAllListResourceModel obj)
        {
            var url2 = Application.Current.Resources["UrlAPI"].ToString();

            var toEmit = new PreRequestAllListResourceModel
            {
                Id = obj.Id,
                RequestType = obj.RequestType
            };

            var response2 = await this.apiService.PutAsync(
                url2,
                "/api/logistica/pre-requerimientos",
                $"/emitir/{obj.Id}",
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

        //



        #region DeleteCommand
        public ICommand DeleteCommand => new RelayCommand<PreRequestAllListResourceModel>(this.Delete);
        private async void Delete(PreRequestAllListResourceModel obj)
        {
            var url2 = Application.Current.Resources["UrlAPI"].ToString();

            var toDelete = new PreRequestAllListResourceModel
            {
                Id = obj.Id,
                RequestType = obj.RequestType
            };
            var response2 = await this.apiService.PostAsync(
            url2,
            "/api/logistica/pre-requerimientos",
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

        //

        private readonly ApiService apiService;

        public PreRequestListDetailViewModel(ObservableCollection<PreRequestAllListResourceModel> list)
        {
            this.apiService = new ApiService();
            this.BPList = list;
        }

        
    }
}
