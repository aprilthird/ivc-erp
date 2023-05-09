using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Views.TechnicalOffice.MixDesigns;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.MixDesigns
{
    public class MixDesignListDetailViewModel : BaseViewModel
    {
        #region PdfMixDesignCommand
        public ICommand PdfMixDesignsCommand => new RelayCommand<MixDesignResourceModel>(this.PdfMixDesigns);
        private async void PdfMixDesigns(MixDesignResourceModel obj)
        {
            MainViewModel.GetInstance().MixDesignPdfViewModel = new MixDesignPdfViewModel(obj.FileUrl);
            await App.Navigator.PushAsync(new MixDesignPdfPage());
        }
        #endregion


        #region BimList
        private ObservableCollection<MixDesignResourceModel> bpList;
        public ObservableCollection<MixDesignResourceModel> BPList
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

        public MixDesignListDetailViewModel(ObservableCollection<MixDesignResourceModel> list)
        {

            this.BPList = list;

        }
    }
}
