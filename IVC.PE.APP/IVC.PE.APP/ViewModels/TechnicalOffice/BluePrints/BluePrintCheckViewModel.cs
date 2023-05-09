using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.TechnicalOffice.BluePrints;
using IVC.PE.APP.Views.TechnicalOffice.BluePrints._Modals;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.BluePrints
{
   public class BluePrintCheckViewModel : BaseViewModel
    {
        #region CaptureQrCommand
        public ICommand CaptureQrCommand => new RelayCommand(this.CaptureQr);
        private async void CaptureQr()
        {
            if (this.CaptureOpt == 1)
            {
                MainViewModel.GetInstance().BluePrintQrCameraViewModel = new BluePrintQrCameraViewModel(this);
                await App.Navigator.PushAsync(new BluePrintQrCameraPage());
            }
            else if (this.CaptureOpt == 2)
            {
                
                var qrCodeReadPopupPage = new BluePrintQrCodeReadPopupPage();
                await PopupNavigation.Instance.PushAsync(qrCodeReadPopupPage);
                var myFileUrl = await qrCodeReadPopupPage.PopupClosedTask;
                if (myFileUrl != null)
                    this.FileUrl = $"https://drive.google.com/viewerng/viewer?embedded=true&url={myFileUrl}";
                var qrcode = await qrCodeReadPopupPage.PopupClosedTask2;
                if (qrcode != null)
                    this.Code = qrcode;
                var bpver = await qrCodeReadPopupPage.PopupClosedTask3;
                if (bpver != null)
                    this.Version = bpver;
            }
        }
        #endregion

        #region ReCapture
        public ICommand ReCaptureCommand => new RelayCommand(this.ReCapture);
        private async void ReCapture()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<BluePrintUspResourceModel>(
                url,
                "/api/oficina-tecnica/planos",
                "/validar-qr?qrString=" + this.Code,
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

            var myPdfUrl = (List<BluePrintUspResourceModel>)response.Result;
            this.AllBluePrints = myPdfUrl;
            Uri tempUri;
            //Version Aprobada
            //Ver Plano (Opcion)
            //Ver Carta (Opcion)
            if (this.Version.Equals("Aprobada"))
            {
                string selectedOption = await App.Current.MainPage.DisplayActionSheet("Version Aprobada", "Cancelar", "",
                            new string[] { "Plano Aprobado", "Carta" });
                //this.MyFileUrl = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();

                switch (selectedOption)
                {
                    //ver plano aprobado
                    //ver carta
                    case "Plano Aprobado":
                        tempUri = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();
                        this.MyFileUrl = tempUri;
                        UpdateWebView(this.MyFileUrl);
                        break;
                    case "Carta":

                        tempUri = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.LetterUrl).FirstOrDefault();
                        this.MyFileUrl = tempUri;
                        UpdateWebView(this.MyFileUrl);
                        break;
                    default:
                        break;
                }

            }
            else if (this.Version.Equals("Contractual") && AllBluePrints.Count() == 1)
            {
                string selectedOption = await App.Current.MainPage.DisplayActionSheet("Version Contractual, Este Plano aun no tiene una Version Aprobada", "Cancelar", "",
                            new string[] { "Ver" });
                //this.MyFileUrl = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();

                switch (selectedOption)
                {
                    //ver plano aprobado
                    //ver carta
                    case "Ver":
                        tempUri = this.AllBluePrints.Where(x => x.Description == "Contractual").Select(x => x.FileUrl).FirstOrDefault();
                        this.MyFileUrl = tempUri;
                        UpdateWebView(this.MyFileUrl);
                        break;
                    default:
                        break;
                }



                //Version Contractual, Este Plano aun no tiene una Version Aprobada
                //, OK , Visualizar



            }
            else if (this.Version.Equals("Contractual") && AllBluePrints.Count() == 2)
            {
                foreach (var item in this.AllBluePrints)
                {
                    if (item.Description == "Aprobada")
                    {
                        //agregar carta
                        string selectedOption = await App.Current.MainPage.DisplayActionSheet("Version Contractual, Existe una versión aprobada", "Cancelar", "",
                            new string[] { "Plano Aprobado", "Carta", "Plano Contractual" });

                        switch (selectedOption)
                        {
                            //ver plano aprobado
                            //ver carta
                            case "Plano Aprobado":
                                tempUri = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();
                                this.MyFileUrl = tempUri;
                                UpdateWebView(this.MyFileUrl);
                                break;
                            case "Carta":
                                tempUri = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.LetterUrl).FirstOrDefault();
                                this.MyFileUrl = tempUri;
                                UpdateWebView(this.MyFileUrl);
                                break;
                            case "Plano Contractual":
                                tempUri = this.AllBluePrints.Where(x => x.Description == "Contractual").Select(x => x.FileUrl).FirstOrDefault(); ;
                                this.MyFileUrl = tempUri;
                                UpdateWebView(this.MyFileUrl);
                                break;
                        }
                    }
                }


            }
            
            //await App.Navigator.PopAsync();
        }
        #endregion

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        private bool isQuestionVisible;

        public bool IsQuestionVisible
        {
            get { return this.isQuestionVisible; }
            set { this.SetValue(ref this.isQuestionVisible,value); }
        }

        private string fileUrl;
        public string FileUrl
        {
            get { return this.fileUrl; }
            set { this.SetValue(ref this.fileUrl, value); }
        }


        //--


        private Uri myFileUrl;
        public Uri MyFileUrl
        {
            get { return this.myFileUrl; }
            set { this.SetValue(ref this.myFileUrl, value); }
        }

        //--

        private string code;

        public string Code
        {
            get { return this.code; }
            set { this.SetValue(ref this.code,value); }
        }

        //--

        private string version;

        public string Version
        {
            get { return this.version; }
            set { this.SetValue(ref this.version, value); }
        }

        //--


        public List<BluePrintUspResourceModel> AllBluePrints;

        private readonly ApiService apiService;
        public int CaptureOpt { get; set; }
        //public ToScanBluePrintViewModel Parent { get; set; }
        public BluePrintCheckViewModel(int _captureOpt)
        {
            this.apiService = new ApiService();
            this.CaptureOpt = _captureOpt;
            this.IsRefreshing = false;
            this.IsQuestionVisible = false;
            //this.IsEnabled = true;
            //this.CaptureOpt = _captureOpt;

        }

        public void UpdateWebView(Uri _uri)
        {
            this.FileUrl = $"https://drive.google.com/viewerng/viewer?embedded=true&url={_uri}";
        }

        public void UpdateCode (string _code,string _version)
        {
            this.Code = _code;
            this.Version = _version;
            this.IsQuestionVisible = true;
        }
    }
}
