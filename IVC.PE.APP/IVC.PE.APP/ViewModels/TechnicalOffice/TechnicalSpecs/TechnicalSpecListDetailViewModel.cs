using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Views.TechnicalOffice.TechnicalSpecs;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.TechnicalSpecs
{
    public class TechnicalSpecListDetailViewModel : BaseViewModel
    {
        #region PdfTechnicalSpecsCommand
        public ICommand PdfTechnicalSpecsCommand => new RelayCommand<TechnicalSpecResourceModel>(this.PdfProviders);
        private async void PdfProviders(TechnicalSpecResourceModel obj)
        {
            MainViewModel.GetInstance().TechnicalSpecPdfViewModel = new TechnicalSpecPdfViewModel(obj.FileUrl);
            await App.Navigator.PushAsync(new TechnicalSpecPdfPage());
        }
        #endregion

        #region BimList
        private ObservableCollection<TechnicalSpecResourceModel> bpList;
        public ObservableCollection<TechnicalSpecResourceModel> BPList
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

        public TechnicalSpecListDetailViewModel(ObservableCollection<TechnicalSpecResourceModel> list)
        {

            this.BPList = list;

        }
    }
}
