using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Views.TechincalOffice.ProviderCatalogs;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.ProviderCatalogs
{
    public class ProviderCatalogListDetailViewModel : BaseViewModel
    {
        #region PdfProvidersCommand
        public ICommand PdfProvidersCommand => new RelayCommand<ProviderCatalogResourceModel>(this.PdfProviders);
        private async void PdfProviders(ProviderCatalogResourceModel obj)
        {
            MainViewModel.GetInstance().ProviderCatalogPdfViewModel = new ProviderCatalogPdfViewModel(obj.FileUrl);
            await App.Navigator.PushAsync(new ProviderCatalogPdfPage());
        }
        #endregion


        #region BimList
        private ObservableCollection<ProviderCatalogResourceModel> bpList;
        public ObservableCollection<ProviderCatalogResourceModel> BPList
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

        public ProviderCatalogListDetailViewModel(ObservableCollection<ProviderCatalogResourceModel> list)
        {

            this.BPList = list;

        }
    }
}
