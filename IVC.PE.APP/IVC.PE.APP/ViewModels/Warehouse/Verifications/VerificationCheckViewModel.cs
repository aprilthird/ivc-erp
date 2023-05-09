using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Warehouse.Verification;
using IVC.PE.APP.Views.Warehouse.Verification._Modals;
using IVC.PE.BINDINGRESOURCES.Areas.EquipmentMachinery;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Warehouse.Verifications
{
    public class VerificationCheckViewModel : BaseViewModel
    {
        #region BpList
        private ObservableCollection<EquipmentMachineryVerificationResourceModel> bpList;
        public ObservableCollection<EquipmentMachineryVerificationResourceModel> BPList
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
                MainViewModel.GetInstance().VerificationQrCameraViewModel = new VerificationQrCameraViewModel(this);
                await App.Navigator.PushAsync(new VerificationQrCameraPage());
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


        public List<EquipmentMachineryVerificationResourceModel> AllBluePrints;

        private readonly ApiService apiService;
        public int CaptureOpt { get; set; }
        //public ToScanBluePrintViewModel Parent { get; set; }
        public VerificationCheckViewModel(int _captureOpt)
        {
            this.apiService = new ApiService();
            this.CaptureOpt = _captureOpt;
            this.IsRefreshing = false;
            

            var myBps = new EquipmentMachineryVerificationResourceModel
            {
                Year = "",
                Model = "",
                Equipment = "",
                Provider = "",

            };

            List<EquipmentMachineryVerificationResourceModel> bplist = new List<EquipmentMachineryVerificationResourceModel>();

            bplist.Add(myBps);

            this.BPList = new ObservableCollection<EquipmentMachineryVerificationResourceModel>(bplist);


            //this.IsEnabled = true;
            //this.CaptureOpt = _captureOpt;

        }

        

        public void UpdateFields (ObservableCollection<EquipmentMachineryVerificationResourceModel> list)
        {
            this.BPList = list;
        }


    }
}
