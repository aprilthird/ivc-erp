using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Views.TechnicalOffice.BluePrints;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.BluePrints
{
   public class BluePrintListDetailViewModel : BaseViewModel
    {
        #region PdfBluePrintsCommand
        public ICommand PdfBluePrintsCommand => new RelayCommand<BluePrintListResourceModel>(this.PdfBluePrints);
        private async void PdfBluePrints(BluePrintListResourceModel obj)
        {
            MainViewModel.GetInstance().BluePrintPdfViewModel = new BluePrintPdfViewModel(obj.FileUrl);
            await App.Navigator.PushAsync(new BluePrintPdfPage());
        }
        #endregion

        #region PdfLettersCommand
        public ICommand PdfLetterCommand => new RelayCommand<BluePrintListResourceModel>(this.PdfLetters);
        private async void PdfLetters(BluePrintListResourceModel obj)
        {
            MainViewModel.GetInstance().LetterPdfViewModel = new LetterPdfViewModel(obj.LetterFileUrl);
            await App.Navigator.PushAsync(new LettersPdfPage());
        }
        #endregion
        #region BluePrintList
        private ObservableCollection<BluePrintListResourceModel> bpList;
        public ObservableCollection<BluePrintListResourceModel> BPList
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

        public BluePrintListDetailViewModel(ObservableCollection<BluePrintListResourceModel> list)
        {
            
            this.BPList= list;   
            
        }
    }
}
