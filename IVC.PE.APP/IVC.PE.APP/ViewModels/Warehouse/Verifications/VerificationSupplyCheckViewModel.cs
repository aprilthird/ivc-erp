using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Warehouse.Verification;
using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.Warehouse.Verifications
{
   public class VerificationSupplyCheckViewModel : BaseViewModel
    {
        #region BpList
        private ObservableCollection<VerificationResourceModel> bpList;
        public ObservableCollection<VerificationResourceModel> BPList
        {
            get { return this.bpList; }
            set { this.SetValue(ref this.bpList, value); }
        }
        #endregion

        #region CaptureQrCommand
        public ICommand CaptureQrCommand => new RelayCommand(this.CaptureQr);
        private async void CaptureQr()
        {
            if (this.CaptureOpt == 1)
            {
                MainViewModel.GetInstance().VerificationSupplyQrCameraViewModel = new VerificationSupplyQrCameraViewModel(this);
                await App.Navigator.PushAsync(new VerificationSupplyQrCameraPage());
            }
            else if (this.CaptureOpt == 2)
            {



            }
        }
        #endregion


        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        //--


        public List<VerificationResourceModel> AllBluePrints;

        private readonly ApiService apiService;
        public int CaptureOpt { get; set; }
        //public ToScanBluePrintViewModel Parent { get; set; }
        public VerificationSupplyCheckViewModel(int _captureOpt)
        {
            this.apiService = new ApiService();
            this.CaptureOpt = _captureOpt;
            this.IsRefreshing = false;


            var myBps = new VerificationResourceModel
            {
                Description = "",

                CorrelativeCodeStr = "",

                IvcCode = "",

                Tradename = "",

                DeliveryDateStr = "",

                RemissionGuide = "",

                Measure = "",

                Sums = ""

        };

            List<VerificationResourceModel> bplist = new List<VerificationResourceModel>();

            bplist.Add(myBps);

            this.BPList = new ObservableCollection<VerificationResourceModel>(bplist);


            //this.IsEnabled = true;
            //this.CaptureOpt = _captureOpt;

        }



        public void UpdateFields(ObservableCollection<VerificationResourceModel> list)
        {
            this.BPList = list;
        }
    }
}
