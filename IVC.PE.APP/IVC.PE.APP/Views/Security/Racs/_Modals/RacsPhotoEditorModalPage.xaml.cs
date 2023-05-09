using IVC.PE.APP.ViewModels;
using IVC.PE.APP.ViewModels.Security.Racs;
using Plugin.Screenshot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IVC.PE.APP.Views.Security.Racs._Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RacsPhotoEditorModalPage : ContentPage
    {
        public string SelectedImage { get; set; }
        public RacsAddViewModel ParentViewModel { get; set; }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
                portraitStack.IsVisible = false;
                landscapeStack.IsVisible = true;
            } else
            {
                portraitStack.IsVisible = true;
                landscapeStack.IsVisible = false;
            }
        }

        public RacsPhotoEditorModalPage(string _selectedImage, RacsAddViewModel _parent)
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
            saveButtonPortrait.IsVisible = false;
            clearButtonPortrait.IsVisible = false;
            saveButtonLandscape.IsVisible = false;
            clearButtonLandscape.IsVisible = false;
            var result = await CrossScreenshot.Current.CaptureAsync();
            var stream = new MemoryStream(result);
            this.ParentViewModel.UpdatePhotoStream(stream);
            //await App.Navigator.PopAsync();
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}