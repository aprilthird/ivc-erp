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
   public class PreRequestIssuedListDetailViewModel : BaseViewModel
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
            MainViewModel.GetInstance().PreRequestDetailListDetailViewModel = new PreRequestDetailListDetailViewModel(obj.Id, obj.ProjectFormulaId,obj.ProjectId);
            await App.Navigator.PushAsync(new PreRequestDetailListDetailPage());
        }

        #endregion
        //








        

        //

        private readonly ApiService apiService;

        public PreRequestIssuedListDetailViewModel(ObservableCollection<PreRequestAllListResourceModel> list)
        {
            this.apiService = new ApiService();
            this.BPList = list;
        }
    }
}
