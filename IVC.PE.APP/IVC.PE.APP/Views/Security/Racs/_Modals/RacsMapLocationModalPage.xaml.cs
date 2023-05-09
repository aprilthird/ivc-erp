using IVC.PE.APP.ViewModels.Security.Racs;
using Plugin.Screenshot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IVC.PE.APP.Views.Security.Racs._Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RacsMapLocationModalPage : ContentPage
    {
        public RacsAddViewModel ParentViewModel { get; set; }

        public RacsMapLocationModalPage(RacsAddViewModel _parent)
        {
            this.ParentViewModel = _parent;
            InitializeComponent();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            saveButton.IsVisible = false;

            await CrossScreenshot.Current.CaptureAndSaveAsync();
            var result = await CrossScreenshot.Current.CaptureAsync();
            var stream = new MemoryStream(result);
            this.ParentViewModel.UpdateLocationStream(stream);
            //await App.Navigator.PopAsync();
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}