using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.EquipmentMachinery;
using IVC.PE.APP.Views.EquipmentMachinery._Modals;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.EquipmentMachinery
{
    public class EquipmentMachPartCheckViewModel : BaseViewModel
    {
        #region CaptureQrCommand
        public ICommand CaptureQrCommand => new RelayCommand(this.CaptureQr);
        private async void CaptureQr()
        {
            if (this.CaptureOpt == 1)
            {
                MainViewModel.GetInstance().EquipmentMachPartQrCameraViewModel = new EquipmentMachPartQrCameraViewModel(this,SelectedOperatorId,OperatorName);
                await App.Navigator.PushAsync(new EquipmentMachPartQrCameraPage());
            }
            else if (this.CaptureOpt == 2)
            {

                var qrCodeReadPopupPage = new EquipmentMachineryQrCodeReadPopupPage(this.SelectedOperatorId,this.OperatorName);
                await PopupNavigation.Instance.PushAsync(qrCodeReadPopupPage);
                //var myFileUrl = await qrCodeReadPopupPage.PopupClosedTask;
                //if (myFileUrl != null)
                //    this.FileUrl = $"https://drive.google.com/viewerng/viewer?embedded=true&url={myFileUrl}";
                //var qrcode = await qrCodeReadPopupPage.PopupClosedTask2;
                //if (qrcode != null)
                //    this.Code = qrcode;
                //var bpver = await qrCodeReadPopupPage.PopupClosedTask3;
                //if (bpver != null)
                //    this.Version = bpver;
            }
        }
        #endregion

        #region CaptureQrCommand
        public ICommand CaptureQrCommand2 => new RelayCommand(this.CaptureQr2);
        private async void CaptureQr2()
        {
            if (this.CaptureOpt == 1)
            {
                MainViewModel.GetInstance().EquipmentMachineryOperatorScanQrCameraViewModel = new EquipmentMachineryOperatorScanQrCameraViewModel(this);
                await App.Navigator.PushAsync(new EquipmentMachineryOperatorScanQrCameraPage());
            }
            else if (this.CaptureOpt == 2)
            {

                var qrCodeReadPopupPage = new EquipmentMachineryOperatorQrCodeReadPopupPage();
                await PopupNavigation.Instance.PushAsync(qrCodeReadPopupPage);
                this.SelectedOperatorId = await qrCodeReadPopupPage.PopupClosedTask;
                this.OperatorName = await qrCodeReadPopupPage.PopupClosedTask2;
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

        public Guid SelectedOperatorId { get; set; }

        public string OperatorName { get; set; }

        //public List<BluePrintUspResourceModel> AllBluePrints;

        private readonly ApiService apiService;
        public int CaptureOpt { get; set; }
        //public ToScanBluePrintViewModel Parent { get; set; }
        public EquipmentMachPartCheckViewModel(int _captureOpt, [Optional] Guid Id, [Optional] string OperatorName)
        {
            this.apiService = new ApiService();
            this.CaptureOpt = _captureOpt;
            this.IsRefreshing = false;
            //this.IsEnabled = true;

        }

        //public void UpdateWebView(Uri _uri)
        //{
        //    this.FileUrl = $"https://drive.google.com/viewerng/viewer?embedded=true&url={_uri}";
        //}

        //public void UpdateCode(string _code, string _version)
        //{
        //    this.Code = _code;
        //    this.Version = _version;
        //    this.IsQuestionVisible = true;
        //}

        public void UpdateOperator(Guid Id, string OperatorName)
        {
            this.OperatorName = OperatorName;
            this.SelectedOperatorId = Id;

        }

        
    }
}
