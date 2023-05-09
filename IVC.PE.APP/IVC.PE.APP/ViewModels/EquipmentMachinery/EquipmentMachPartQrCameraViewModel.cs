using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.EquipmentMachinery;
using IVC.PE.BINDINGRESOURCES.Areas.EquipmentMachinery;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;

namespace IVC.PE.APP.ViewModels.EquipmentMachinery
{
    public class EquipmentMachPartQrCameraViewModel : BaseViewModel
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
                    await App.Current.MainPage.DisplayAlert("Lectura", barcode , "Ok");
                    var url = Application.Current.Resources["UrlAPI"].ToString();

                    var response = await this.apiService.GetListAsync<EquipmentMachPartConsultingResourceModel>(
                        url,
                        "/api/equipos/parte-equipos-maquinaria",
                        "/consultar-qr?qrString=" + barcode+
                        "&month="+DateTime.UtcNow.Date.Month+
                        "&year=" + DateTime.UtcNow.Date.Year,
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

                    var list = (List<EquipmentMachPartConsultingResourceModel>)response.Result;
                    this.All = list;


                    if(All.Count > 0)
                    {
                        string selectedOption = await App.Current.MainPage.DisplayActionSheet("Existe un registro previo", "Cancelar", "",
                                    new string[] { "Continuar" });

                        switch (selectedOption)
                        {
                            case "Continuar":
                                MainViewModel.GetInstance().EquipmentMachPartFoldingViewModel = new EquipmentMachPartFoldingViewModel(barcode,SelectedOperatorId,OperatorName);
                                await App.Navigator.PushAsync(new EquipmentMachPartFoldingPage());
                                break;
                            default:
                                break;
                        }
                    }
                    else if (All.Count == 0)
                    {
                        string selectedOption = await App.Current.MainPage.DisplayActionSheet("No Existe un registro previo", "Cancelar", "",
                                    new string[] { "Continuar" });

                        switch (selectedOption)
                        {
                            case "Continuar":
                                var toSave = new EquipmentMachPartResourceModel
                                {
                                    ProjectId = Guid.Parse("CB9CD712-E2DB-421A-52F0-08D88325D938"),
                                    Month = DateTime.UtcNow.Date.Month,
                                    Year = DateTime.UtcNow.Date.Year,
                                };

                                var url2 = Application.Current.Resources["UrlAPI"].ToString();
                                var response2 = await this.apiService.PostAsync(
                                    url,
                                    "/api/equipos/parte-equipos-maquinaria",
                                    "/parte-padre/registrar?qrString=" + barcode,
                                    toSave,
                                    "bearer",
                                    MainViewModel.GetInstance().Token.Access_Token
                                    );

                                
                                if (!response2.IsSuccess)
                                {
                                    await Application.Current.MainPage.DisplayAlert(
                                        ConstantHelpers.ResponseMessages.FAIL,
                                        response2.Message,
                                        ConstantHelpers.ResponseMessages.OK
                                        );
                                    return;
                                }

                                await Application.Current.MainPage.DisplayAlert(
                                        ConstantHelpers.ResponseMessages.SUCCESS,
                                        ConstantHelpers.ResponseMessages.SUCCESS_ADDED,
                                        ConstantHelpers.ResponseMessages.OK
                                        );
                                await App.Navigator.PopAsync();

                                MainViewModel.GetInstance().EquipmentMachPartFoldingViewModel = new EquipmentMachPartFoldingViewModel(barcode,SelectedOperatorId,OperatorName);
                                await App.Navigator.PushAsync(new EquipmentMachPartFoldingPage());
                                break;
                            default:
                                break;
                        }
                    }

                    //await App.Navigator.PopAsync();
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

        public Guid SelectedOperatorId { get; set; }

        public string OperatorName { get; set; }

        public List<EquipmentMachPartConsultingResourceModel> All;
        public EquipmentMachPartQrCameraViewModel(EquipmentMachPartCheckViewModel _parent, Guid SelectedOperatorId, string OperatorName)
        {
            this.apiService = new ApiService();
            this.Parent = _parent;
            this.IsScanning = true;
            this.IsAnalyzing = true;
            this.SelectedOperatorId = SelectedOperatorId;
            this.OperatorName = OperatorName;
            this.All = new List<EquipmentMachPartConsultingResourceModel>();
        }
    }
}
