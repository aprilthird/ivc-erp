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
    public partial class RacsLiftPhotoEditoModalPage : ContentPage
    {
        public string SelectedImage { get; set; }
        public RacsLiftViewModel ParentViewModel { get; set; }

        public RacsLiftPhotoEditoModalPage(string _selectedImage, RacsLiftViewModel _parent)
        {
            this.SelectedImage = _selectedImage;
            this.ParentViewModel = _parent;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            imagebackground.Source = ImageSource.FromFile(_selectedImage);
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            signaturepad.Clear();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            saveButton.IsVisible = false;
            clearButton.IsVisible = false;
            var result = await CrossScreenshot.Current.CaptureAsync();
            var stream = new MemoryStream(result);
            this.ParentViewModel.UpdateLiftPhotoStream(stream);
            //await App.Navigator.PopAsync();
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}