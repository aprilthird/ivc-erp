using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Views.TechnicalOffice.ErpManuals;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.ErpManuals
{
   public  class ErpManualListDetailViewModel : BaseViewModel
    {
        #region PdfErpManualsCommand
        public ICommand PdfErpManualsCommand => new RelayCommand<ErpManualResourceModel>(this.PdfErpManuals);
        private async void PdfErpManuals(ErpManualResourceModel obj)
        {
            MainViewModel.GetInstance().ErpManualPdfViewModel = new ErpManualPdfViewModel(obj.FileUrl);
            await App.Navigator.PushAsync(new ErpManualPdfPage());
        }
        #endregion


        #region BimList
        private ObservableCollection<ErpManualResourceModel> bpList;
        public ObservableCollection<ErpManualResourceModel> BPList
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
        public ErpManualListDetailViewModel(ObservableCollection<ErpManualResourceModel> list)
        {

            this.BPList = list;

        }
    }
}
