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
    public partial class EquipmentMachineryOperatorQrCodeReadPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly ApiService apiService;
        private TaskCompletionSource<Guid> _taskCompletionSource;
        public Task<Guid> PopupClosedTask => _taskCompletionSource.Task;

        private TaskCompletionSource<string> _taskCompletionSource2;
        public Task<string> PopupClosedTask2 => _taskCompletionSource2.Task;

        public Guid OpId { get; set; }

        public string OpName { get; set; }
        public EquipmentMachineryOperatorQrCodeReadPopupPage()
        {
            this.apiService = new ApiService();
            InitializeComponent();

            //CloseWhenBackgroundIsClicked = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            qrCode.Focus();
            _taskCompletionSource = new TaskCompletionSource<Guid>();
            _taskCompletionSource2 = new TaskCompletionSource<string>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _taskCompletionSource.SetResult(this.OpId);
            _taskCompletionSource2.SetResult(this.OpName);

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
            if (qrCode.Text.Length < 8)
            {
                await Application.Current.MainPage.DisplayAlert(
    "Error de lectura",
    "El codigo no tiene 8 digitos",
    "Entendido"
    );
                qrCode.Text = "";
                return;
            }

            else
            {
                var url = Application.Current.Resources["UrlAPI"].ToString();
                //var url = Application.Current.Resources["UrlAPI"].ToString();

                var response = await this.apiService.GetListAsync<EquipmentMachineryOperatorResourceModel>(
                            url,
                            "/api/equipos/parte-equipos-maquinaria",
                            "/consultar-operador-qr?qrString=" + qrCode.Text,
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

                this.OpId = list.FirstOrDefault().Id;
                this.OpName = list.FirstOrDefault().ActualName;
                MainViewModel.GetInstance().EquipmentMachPartCheckViewModel = new EquipmentMachPartCheckViewModel(2, Id: list.FirstOrDefault().Id, OperatorName: list.FirstOrDefault().ActualName);
                await PopupNavigation.PopAsync(true);

            }


        }
    }
}