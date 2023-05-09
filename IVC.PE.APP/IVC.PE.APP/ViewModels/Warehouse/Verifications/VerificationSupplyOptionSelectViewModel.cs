using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Warehouse.Verification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.Warehouse.Verifications
{
    public class VerificationSupplyOptionSelectViewModel : BaseViewModel
    {
        #region GetDevice
        public ICommand GetDeviceCommand => new RelayCommand(this.GetDevice);
        private async void GetDevice()
        {
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("¿Desde dónde desea leer los codigos?", "Cancelar", "",
                new string[] { "Cámara", "Dispositivo externo" });

            var captureOpt = 0;

            switch (selectedOption)
            {
                case "Cámara":
                    captureOpt = 1;
                    break;
                case "Dispositivo externo":
                    break;
                default:
                    break;
            }

            if (captureOpt != 0)
            {
                MainViewModel.GetInstance().VerificationSupplyCheckViewModel = new VerificationSupplyCheckViewModel(captureOpt);
                await App.Navigator.PushAsync(new VerificationSupplyCheckPage());
            }
        }
        #endregion

        private readonly ApiService apiService;
        public VerificationSupplyOptionSelectViewModel()
        {
            this.apiService = new ApiService();
        }
    }
}
