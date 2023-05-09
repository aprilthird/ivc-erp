using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using IVC.PE.BINDINGRESOURCES.Areas.EquipmentMachinery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;

namespace IVC.PE.APP.ViewModels.EquipmentMachinery
{
    public class EquipmentMachineryOperatorScanQrCameraViewModel : BaseViewModel
    {
        #region HandleScanResultCommand
        public ICommand HandleScanResultCommand => new RelayCommand(this.HandleScanResult);
        private void HandleScanResult()
        {
            this.IsScanning = false;
            this.IsAnalyzing = false;

            Device.BeginInvokeOnMainThread(async () =>
            {
                var barcode = this.ScanResult.Text;


                if (!string.IsNullOrEmpty(barcode))
                {
                    await App.Current.MainPage.DisplayAlert("Lectura", barcode, "Ok");
                    var url = Application.Current.Resources["UrlAPI"].ToString();

                    var response = await this.apiService.GetListAsync<EquipmentMachineryOperatorResourceModel>(
                        url,
                        "/api/equipos/parte-equipos-maquinaria",
                        "/consultar-operador-qr?qrString=" + barcode,
                        "bearer",
                        MainViewModel.GetInstance().Token.Access_Token);

                    if (!response.IsSuccess)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            response.Message,
                            "Entendido"
                            );
                        return;
                    }

                    var list = (List<EquipmentMachineryOperatorResourceModel>)response.Result;
                    this.All = list;

                    Parent.UpdateOperator(this.All.FirstOrDefault().Id, this.All.FirstOrDefault().ActualName);
                    await App.Navigator.PopAsync();

                }
            });

            this.IsScanning = true;
            this.IsAnalyzing = true;
        }
        #endregion

        #region IsScanning
        private bool isScanning;
        public bool IsScanning
        {
            get { return this.isScanning; }
            set { this.SetValue(ref this.isScanning, value); }
        }
        #endregion

        #region IsScanning
        private bool isAnalyzing;
        public bool IsAnalyzing
        {
            get { return this.isAnalyzing; }
            set { this.SetValue(ref this.isAnalyzing, value); }
        }
        #endregion


        private readonly ApiService apiService;
        public EquipmentMachPartCheckViewModel Parent { get; set; }
        public Result ScanResult { get; set; }

        public List<EquipmentMachineryOperatorResourceModel> All;
        public EquipmentMachineryOperatorScanQrCameraViewModel(EquipmentMachPartCheckViewModel _parent)
        {
            this.apiService = new ApiService();
            this.Parent = _parent;
            this.IsScanning = true;
            this.IsAnalyzing = true;
            this.All = new List<EquipmentMachineryOperatorResourceModel>();
        }
    }
}
