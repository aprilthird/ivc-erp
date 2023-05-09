using IVC.PE.APP.Common.Services;
using IVC.PE.APP.ViewModels;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IVC.PE.APP.Views.TechnicalOffice.BluePrints._Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluePrintQrCodeReadPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private TaskCompletionSource<Uri> _taskCompletionSource;
        public Task<Uri> PopupClosedTask => _taskCompletionSource.Task;
        private TaskCompletionSource<string> _taskCompletionSource2;
        public Task<string> PopupClosedTask2 => _taskCompletionSource2.Task;

        private TaskCompletionSource<string> _taskCompletionSource3;
        public Task<string> PopupClosedTask3 => _taskCompletionSource3.Task;
        public Uri MyFileUrl { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        private readonly ApiService apiService;
        public BluePrintQrCodeReadPopupPage()
        {
            this.apiService = new ApiService();
            //this.MyFileUrl = new Uri(string.Empty);
            InitializeComponent();

            //CloseWhenBackgroundIsClicked = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            qrCode.Focus();
            _taskCompletionSource = new TaskCompletionSource<Uri>();
            _taskCompletionSource2 = new TaskCompletionSource<string>();
            _taskCompletionSource3 = new TaskCompletionSource<string>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _taskCompletionSource.SetResult(this.MyFileUrl);
            _taskCompletionSource2.SetResult(this.Code);
            _taskCompletionSource2.SetResult(this.Version);
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed

            return true;
            //return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }

        private void qrCode_Unfocused(object sender, FocusEventArgs e)
        {
            qrCode.Focus();
        }

        private async void searchQrCode_Clicked(object sender, EventArgs e)
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();

            var myQrCode = qrCode.Text.Split('/');
            var bpCode = myQrCode[0];
            this.Code = bpCode;
            var bpVer = myQrCode[1];
            this.Version = bpVer;
            var response = await this.apiService.GetListAsync<BluePrintResourceModel>(
                url,
                "/api/oficina-tecnica/planos",
                "/validar-qr?qrString=" + bpCode,
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
            Uri tempUrl;
            //if (bpVer.Equals("Aprobada"))
            //    this.MyFileUrl = myPdfUrl.Where(x=>x.Description == "Aprobada").Select(x=>x.FileUrl).FirstOrDefault();
            //else
            //{
            //    var contractualBp = myPdfUrl.Where(x => x.Description == "Contractual").Select(x => x.FileUrl).FirstOrDefault();
            //    var aprobadaBp = myPdfUrl.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();

            //    if (aprobadaBp != null)
            //    {
            //        string selectedOption = await App.Current.MainPage.DisplayActionSheet("Existe un plano aprobado,¿Desea visualizarlo?", "Cancelar", "",
            //                new string[] { "Sí" });

            //        switch (selectedOption)
            //        {
            //            case "Sí":
            //                this.MyFileUrl = aprobadaBp;
            //                break;
            //            default:
            //                this.MyFileUrl = contractualBp;
            //                break;
            //        }
            //    } else
            //    {
            //        this.MyFileUrl = contractualBp;
            //    }
            //}

            if (bpVer.Equals("Aprobada"))
            {
                string selectedOption = await App.Current.MainPage.DisplayActionSheet("Version Aprobada", "Cancelar", "",
                            new string[] { "Plano Aprobado", "Carta" });
                //this.MyFileUrl = this.myPdfUrl.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();

                switch (selectedOption)
                {
                    //ver plano aprobado
                    //ver carta
                    case "Plano Aprobado":
                        tempUrl = myPdfUrl.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();
                        this.MyFileUrl = tempUrl;
                        break;
                    case "Carta":
                        tempUrl = myPdfUrl.Where(x => x.Description == "Aprobada").Select(x => x.LetterUrl).FirstOrDefault();
                        this.MyFileUrl = tempUrl;
                        break;
                    default:
                        break;
                }

            }
            else if (bpVer.Equals("Contractual") && myPdfUrl.Count() == 1)
            {
                string selectedOption = await App.Current.MainPage.DisplayActionSheet("Version Contractual, Este Plano aun no tiene una Version Aprobada", "Cancelar", "",
                            new string[] { "Ver" });
                //this.MyFileUrl = this.myPdfUrl.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();

                switch (selectedOption)
                {
                    //ver plano aprobado
                    //ver carta
                    case "Ver":
                        tempUrl = myPdfUrl.Where(x => x.Description == "Contractual").Select(x => x.FileUrl).FirstOrDefault();
                        this.MyFileUrl = tempUrl;
                        break;
                    default:
                        break;
                }



                //Version Contractual, Este Plano aun no tiene una Version Aprobada
                //, OK , Visualizar



            }
            else if (bpVer.Equals("Contractual") && myPdfUrl.Count() == 2)
            {
                foreach (var item in myPdfUrl)
                {
                    if (item.Description == "Aprobada")
                    {
                        //agregar carta
                        string selectedOption = await App.Current.MainPage.DisplayActionSheet("Version Contractual, Existe una versión aprobada", "Cancelar", "",
                            new string[] { "Plano Aprobado", "Carta" , "Plano Contractual"});

                        switch (selectedOption)
                        {
                            //ver plano aprobado
                            //ver carta
                            case "Plano Aprobado":
                                tempUrl = myPdfUrl.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();
                                this.MyFileUrl = tempUrl;
                                break;
                            case "Carta":
                                tempUrl = myPdfUrl.Where(x => x.Description == "Aprobada").Select(x => x.LetterUrl).FirstOrDefault();
                                this.MyFileUrl = tempUrl;
                                break;
                            case "Plano Contractual":
                                tempUrl = myPdfUrl.Where(x => x.Description == "Contractual").Select(x => x.FileUrl).FirstOrDefault();
                                this.MyFileUrl = myPdfUrl.Where(x => x.Description == "Contractual").Select(x => x.FileUrl).FirstOrDefault();
                                break;
                        }
                    }
                }


            }

            await App.Navigator.PopAsync();
            //this.MyFileUrl = myPdfUrl;
        }
    }
}