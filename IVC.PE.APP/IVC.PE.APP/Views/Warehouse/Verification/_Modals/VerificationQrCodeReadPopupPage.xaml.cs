using IVC.PE.APP.Common.Services;
using IVC.PE.APP.ViewModels;
using IVC.PE.BINDINGRESOURCES.Areas.EquipmentMachinery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IVC.PE.APP.Views.Warehouse.Verification._Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerificationQrCodeReadPopupPage : Rg.Plugins.Popup.Pages.PopupPage        
    {
        private TaskCompletionSource<List<EquipmentMachineryVerificationResourceModel>> _taskCompletionSource;
        public Task<List<EquipmentMachineryVerificationResourceModel>> PopupClosedTask => _taskCompletionSource.Task;

        public List<EquipmentMachineryVerificationResourceModel> obj;

        private readonly ApiService apiService;
        public VerificationQrCodeReadPopupPage()
        {
            this.apiService = new ApiService();
            //this.MyFileUrl = new Uri(string.Empty);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            qrCode.Focus();
            _taskCompletionSource = new TaskCompletionSource<List<EquipmentMachineryVerificationResourceModel>>();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _taskCompletionSource.SetResult(this.obj);
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

            var myQrCode = qrCode.Text;

            var response = await this.apiService.GetListAsync<EquipmentMachineryVerificationResourceModel>(
                url,
                "/api/almacenes/verificacion",
                "/listar?equipmentQr=" + myQrCode,
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

            var myObj = (List<EquipmentMachineryVerificationResourceModel>)response.Result;
            this.obj = myObj;





            await App.Navigator.PopAsync();
            //this.MyFileUrl = myPdfUrl;
        }
    }
}