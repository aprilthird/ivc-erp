using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;

namespace IVC.PE.APP.ViewModels.Warehouse.Verifications
{
    public class VerificationSupplyQrCameraViewModel : BaseViewModel
    {
        #region HandleScanResultCommand
        public ICommand HandleScanResultCommand => new RelayCommand(this.HandleScanResult);
        private void HandleScanResult()
        {
            this.IsScanning = false;
            this.IsAnalyzing = false;

            Device.BeginInvokeOnMainThread(async () =>
            {
                var barcode = this.ScanResult.Text.Split('/');

                string code = barcode[0].ToString();

                string remissionGuide = barcode[1].ToString();

                if (!string.IsNullOrEmpty(code))
                {
                    await App.Current.MainPage.DisplayAlert("Lectura", code+"-"+remissionGuide, "Ok");
                    var url = Application.Current.Resources["UrlAPI"].ToString();

                    var response = await this.apiService.GetListAsync<VerificationResourceModel>(
                        url,
                        "/api/almacenes/verificacion",
                        "/listar-items?fullcodeQr=" + code +
                        "&remissionQr="+remissionGuide,
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

                    var myBps = (List<VerificationResourceModel>)response.Result;
                    this.BPList = new ObservableCollection<VerificationResourceModel>(myBps);


                    //Version Aprobada
                    //Ver Plano (Opcion)
                    //Ver Carta (Opcion)

                    Parent.UpdateFields(this.BPList);

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
        public VerificationSupplyCheckViewModel Parent { get; set; }
        public Result ScanResult { get; set; }


        private ObservableCollection<VerificationResourceModel> bpList;
        public ObservableCollection<VerificationResourceModel> BPList
        {
            get { return this.bpList; }
            set { this.SetValue(ref this.bpList, value); }
        }
        public VerificationSupplyQrCameraViewModel(VerificationSupplyCheckViewModel _parent)
        {
            this.apiService = new ApiService();
            this.Parent = _parent;
            this.IsScanning = true;
            this.IsAnalyzing = true;

        }
    }
}
