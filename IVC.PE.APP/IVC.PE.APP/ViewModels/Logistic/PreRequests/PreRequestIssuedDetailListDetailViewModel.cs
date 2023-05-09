using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Services;
using IVC.PE.BINDINGRESOURCES.Areas.Logistic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Logistic.PreRequests
{
    public class PreRequestIssuedDetailListDetailViewModel : BaseViewModel
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




        //
        public PreRequestIssuedDetailListDetailViewModel(Guid preId, Guid formulaId, Guid familyId, Guid projectId)
        {
            this.apiService = new ApiService();
            this.PreRequestId = preId;
            this.FormulaId = formulaId;
            this.FamilyId = familyId;
            this.ProjectId = projectId;
            LoadDetails();

        }
    }
}
