//using GalaSoft.MvvmLight.Command;
//using IVC.PE.APP.Common.Services;
//using IVC.PE.APP.Views.TechnicalOffice.BluePrints;
//using IVC.PE.APP.Views.TechnicalOffice.BluePrints._Modals;
//using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
//using Rg.Plugins.Popup.Services;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Text;
//using System.Windows.Input;
//using Xamarin.Essentials;
//using Xamarin.Forms;


//namespace IVC.PE.APP.ViewModels.TechnicalOffice.BluePrints
//{
//    public class ToScanBluePrintViewModel : BaseViewModel
//    {
//        //#region AttendanceRegistry
//        //public ICommand AttendanceRegistryCommand => new RelayCommand(this.AttendanceRegistry);
//        //private async void AttendanceRegistry()
//        //{
//        //    string selectedOption = await App.Current.MainPage.DisplayActionSheet("¿Desde dónde desea leer los codigos?", "Cancelar", "",
//        //        new string[] { "Cámara", "Dispositivo externo" });

//        //    var captureOpt = 0;

//        //    switch (selectedOption)
//        //    {
//        //        case "Cámara":
//        //            captureOpt = 1;
//        //            break;
//        //        case "Dispositivo externo":
//        //            captureOpt = 2;
//        //            break;
//        //        default:
//        //            break;
//        //    }

//        //    if (captureOpt != 0)
//        //    {
//        //        MainViewModel.GetInstance().WorkerAttendanceCheckViewModel = new WorkerAttendanceCheckViewModel(captureOpt, this.SelectedDate.ToString("dd/MM/yyyy"), this.SelectedSewerGroup.Id, this.SelectedProject.Id);
//        //        await App.Navigator.PushAsync(new WorkerAttendanceCheckPage());
//        //    }
//        //}
//        //#endregion

//        #region BluePrintList
//        private ObservableCollection<BluePrintListResourceModel> bpList;
//        public ObservableCollection<BluePrintListResourceModel> BPList
//        {
//            get { return this.bpList; }
//            set { this.SetValue(ref this.bpList, value); }
//        }
       
//        #endregion

//        #region QRCode
//        private string code;
//        public string Code
//        {
//            get { return this.code; }
//            set
//            {
//                this.SetValue(ref this.code, value);
//            }
//        }

//        private bool isRefreshing;
//        public bool IsRefreshing
//        {
//            get { return this.isRefreshing; }
//            set { this.SetValue(ref this.isRefreshing, value); }
//        }
//        #endregion

//        #region LoadPdfCommand
//        public ICommand LoadPdfCommand => new RelayCommand(this.LoadPdfs);
//        private async void LoadPdfs()
//        {
//            var url = Application.Current.Resources["UrlAPI"].ToString();

//            var response = await this.apiService.GetAsync<Uri>(
//                url,
//                "/api/oficina-tecnica/planos",
//                "/lectura-qr?qrString=" + this.Code,
//                "bearer",
//                MainViewModel.GetInstance().Token.Access_Token);

//            if (!response.IsSuccess)
//            {
//                await Application.Current.MainPage.DisplayAlert(
//                    "Error",
//                    response.Message,
//                    "Entendido"
//                    );
//                return;
//            }

//            var myPdfUrl = (Uri)response.Result;

//            //this.PdfUrl = $"{url}/oficina-tecnica/planos/lectura-qr?qrString="+this.Code;
//            this.FileUrl = $"https://drive.google.com/viewerng/viewer?embedded=true&url={myPdfUrl}";
//        }

//        #endregion

//        #region CaptureQrCodeCommand
//        public ICommand CaptureQrCodeCommand => new RelayCommand(this.CaptureQrCode);
//        private async void CaptureQrCode()
//        {
//            string selectedOption = await App.Current.MainPage.DisplayActionSheet("¿Desde dónde desea leer los codigos?", "Cancelar", "",
//                new string[] { "Cámara", "Dispositivo externo" });

//            switch (selectedOption)
//            {
//                case "Cámara":
//                    GoToCamera();
//                    break;
//                case "Dispositivo externo":
//                    GoToQrLector();
//                    break;
//                default:
//                    break;
//            }
//        }

//        private async void GoToQrLector()
//        {
//            var qrCodeReadPopupPage = new BluePrintQrCodeReadPopupPage();
//            await PopupNavigation.Instance.PushAsync(qrCodeReadPopupPage);
//            var myFileUrl = await qrCodeReadPopupPage.PopupClosedTask;
//            if (myFileUrl != null)
//                this.FileUrl = $"https://drive.google.com/viewerng/viewer?embedded=true&url={myFileUrl}";
//        }

//        private async void GoToCamera()
//        {
//            MainViewModel.GetInstance().BluePrintQrCameraViewModel = new BluePrintQrCameraViewModel(this);
//            await App.Navigator.PushAsync(new BluePrintQrCameraPage());
//        }
//        #endregion

//        private string fileUrl;
//        public string FileUrl {
//            get { return this.fileUrl; }
//            set { this.SetValue(ref this.fileUrl, value); }
//        }

//        private readonly ApiService apiService;

//        public ToScanBluePrintViewModel()
//        {
//            this.apiService = new ApiService();
//            this.IsRefreshing = false;
//        }

//        public void UpdateWebView(Uri _uri)
//        {
//            this.FileUrl = $"https://drive.google.com/viewerng/viewer?embedded=true&url={_uri}";
//        }
//    }
//}
