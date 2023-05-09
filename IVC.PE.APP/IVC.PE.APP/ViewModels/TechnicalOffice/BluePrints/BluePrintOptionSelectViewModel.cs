using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.TechnicalOffice.BluePrints;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.BluePrints
{
    
    public class BluePrintOptionSelectViewModel : BaseViewModel
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
                    captureOpt = 2;
                    break;
                default:
                    break;
            }

            if (captureOpt != 0)
            {
                MainViewModel.GetInstance().BluePrintCheckViewModel = new BluePrintCheckViewModel(captureOpt);
                await App.Navigator.PushAsync(new BluePrintCheckPage());
            }
        }
        #endregion

        private readonly ApiService apiService;
        public BluePrintOptionSelectViewModel()
        {
            this.apiService = new ApiService();
        }
    }
}
