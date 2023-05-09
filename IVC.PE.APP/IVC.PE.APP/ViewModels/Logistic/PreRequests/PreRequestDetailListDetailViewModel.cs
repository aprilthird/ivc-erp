using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
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
    public class PreRequestDetailListDetailViewModel : BaseViewModel
    {
        #region BluePrintList
        private ObservableCollection<PreRequestDetailListResourceModel> bpList;
        public ObservableCollection<PreRequestDetailListResourceModel> BPList
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
            var response = await this.apiService.GetListAsync<PreRequestDetailListResourceModel>(
                url,
                "/api/logistica/pre-requerimientos",
                "/detalles/listar?reqId=" + this.PreRequestId,
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

            var myBps = (List<PreRequestDetailListResourceModel>)response.Result;
            this.BPList = new ObservableCollection<PreRequestDetailListResourceModel>(myBps);

        }

        #endregion
        //
        public Guid PreRequestId { get; set; }

        public Guid FormulaId { get; set; }
        public Guid FamilyId { get; set; }
        public Guid ProjectId { get; set; }
   

        private readonly ApiService apiService;

        #region DetailEditCommand
        public ICommand DetailEditCommand => new RelayCommand<PreRequestDetailListResourceModel>(this.Detail);
        private async void Detail(PreRequestDetailListResourceModel obj)
        {
            MainViewModel.GetInstance().PreRequestDetailEditViewModel = new PreRequestDetailEditViewModel(obj.Id,this.FormulaId, this.FamilyId, this.PreRequestId, this.ProjectId);
            await App.Navigator.PushAsync(new PreRequestDetailEditPage());
        }
        #endregion
        //



        #region DeleteCommand
        public ICommand DeleteCommand => new RelayCommand<PreRequestDetailListResourceModel>(this.Delete);
        private async void Delete(PreRequestDetailListResourceModel obj)
        {
            var url2 = Application.Current.Resources["UrlAPI"].ToString();

            var toDelete = new PreRequestDetailListResourceModel
            {
                Id = obj.Id,
            };
            var response2 = await this.apiService.PostAsync(
            url2,
            "/api/logistica/pre-requerimientos",
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
        public PreRequestDetailListDetailViewModel(Guid preId, Guid formulaId, Guid projectId)
        {
            this.apiService = new ApiService();
            this.PreRequestId = preId;
            this.FormulaId = formulaId;
            this.ProjectId = projectId;
            LoadDetails();

        }
    }
}
