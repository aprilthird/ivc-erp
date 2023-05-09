using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Views.TechnicalOffice.Bims;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.Bims
{
    public class BimListDetailViewModel : BaseViewModel
    {
        #region PdfBimCommand
        public ICommand PdfBimsCommand => new RelayCommand<BimResourceModel>(this.PdfBims);
        private async void PdfBims(BimResourceModel obj)
        {
            MainViewModel.GetInstance().BimPdfViewModel = new BimPdfViewModel(obj.FileUrl);
            await App.Navigator.PushAsync(new BimPdfPage());
        }
        #endregion


        #region BimList
        private ObservableCollection<BimResourceModel> bpList;
        public ObservableCollection<BimResourceModel> BPList
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

        public BimListDetailViewModel(ObservableCollection<BimResourceModel> list)
        {

            this.BPList = list;

        }
    }
}
