using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.ErpManuals
{
    public class ErpManualPdfViewModel : BaseViewModel
    {
        #region DownloadPdfCommand
        public ICommand DownloadPdfCommand => new RelayCommand(this.DonwloadPdf);
        private async void DonwloadPdf()
        {
            await Launcher.OpenAsync(this.FileUrl);
        }
        #endregion

        public string PdfUrl { get; set; }
        public Uri FileUrl { get; set; }

        public ErpManualPdfViewModel(Uri url)
        {
            //var url = Application.Current.Resources["UrlAPI"].ToString();
            this.FileUrl = url;
            this.PdfUrl = $"https://drive.google.com/viewerng/viewer?embedded=true&url={url}";
        }
    }
}
