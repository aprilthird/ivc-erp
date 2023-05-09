using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.ViewModels;
using IVC.PE.APP.ViewModels.EquipmentMachinery;
using IVC.PE.BINDINGRESOURCES.Areas.EquipmentMachinery;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IVC.PE.APP.Views.EquipmentMachinery._Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EquipmentMachineryQrCodeReadPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        
        private readonly ApiService apiService;

        public Guid OpId { get; set; }

        public string OpName { get; set; }
        public EquipmentMachineryQrCodeReadPopupPage(Guid OpId, string OpName)
        {
            this.apiService = new ApiService();
            InitializeComponent();
            this.OpId = OpId;
            this.OpName = OpName;


            //CloseWhenBackgroundIsClicked = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            qrCode.Focus();
            
        }

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

        [Obsolete]
        private async void searchQrCode_Clicked(object sender, EventArgs e)
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            //var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<EquipmentMachPartConsultingResourceModel>(
                       url,
                       "/api/equipos/parte-equipos-maquinaria",
                       "/consultar-qr?qrString=" + qrCode.Text +
                       "&month=" + DateTime.UtcNow.Date.Month +
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



            if (list.Count > 0)
            {
                string selectedOption = await App.Current.MainPage.DisplayActionSheet("Existe un registro previo", "Cancelar", "",
                            new string[] { "Continuar" });

                switch (selectedOption)
                {
                    case "Continuar":
                        MainViewModel.GetInstance().EquipmentMachPartFoldingViewModel = new EquipmentMachPartFoldingViewModel(qrCode.Text,this.OpId,this.OpName);
                        await App.Navigator.PushAsync(new EquipmentMachPartFoldingPage());

                        await PopupNavigation.PopAsync(true);
                        break;
                    default:
                        break;
                }
            }
            else if (list.Count == 0)
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
                            "/parte-padre/registrar?qrString=" + qrCode.Text,
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


                        MainViewModel.GetInstance().EquipmentMachPartFoldingViewModel = new EquipmentMachPartFoldingViewModel(qrcode: qrCode.Text,this.OpId, this.OpName);
                        await App.Navigator.PushAsync(new EquipmentMachPartFoldingPage());

                        await PopupNavigation.PopAsync(true);
                        break;
                    default:
                        break;
                }
            }



        }
    }
}