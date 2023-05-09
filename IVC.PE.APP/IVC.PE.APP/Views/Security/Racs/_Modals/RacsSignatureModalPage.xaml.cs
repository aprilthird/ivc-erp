using IVC.PE.APP.ViewModels.Security.Racs;
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
    public partial class RacsSignatureModalPage : ContentPage
    {
        public RacsAddViewModel ParentViewModel { get; set; }

        public RacsSignatureModalPage(RacsAddViewModel _parent)
        {
            this.ParentViewModel = _parent;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            signaturepad.Clear();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            saveButton.IsVisible = false;
            clearButton.IsVisible = false;
            var stream = await signaturepad.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);
            var memoryStream = new MemoryStream(ReadFully(stream));
            this.ParentViewModel.UpdateSignatureStream(memoryStream);
            //await App.Navigator.PopAsync();
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private byte[] ReadFully(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}