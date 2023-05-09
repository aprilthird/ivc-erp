using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Warehouse.Verification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.Warehouse.Verifications
{
   public class VerificationOptionSelectViewModel : BaseViewModel
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
                MainViewModel.GetInstance().VerificationCheckViewModel = new VerificationCheckViewModel(captureOpt);
                await App.Navigator.PushAsync(new VerificationCheckPage());
            }
        }
        #endregion

        private readonly ApiService apiService;
        public VerificationOptionSelectViewModel()
        {
            this.apiService = new ApiService();
        }
    }
}
