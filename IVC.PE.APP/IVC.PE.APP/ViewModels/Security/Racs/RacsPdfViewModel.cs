namespace IVC.PE.APP.ViewModels.Security.Racs
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class RacsPdfViewModel : BaseViewModel
    {
        #region DownloadPdfCommand
        public ICommand DownloadPdfCommand => new RelayCommand(this.DonwloadPdf);
        private async void DonwloadPdf()
        {
            var url = $"https://erp-ivc-pdf.azurewebsites.net/api/functionapp?url={PdfUrl}";
            await Launcher.OpenAsync(url);
        }
        #endregion

        public string PdfUrl { get; set; }
        public RacsPdfViewModel(Guid racsId)
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            this.PdfUrl = $"{url}/seguridad/racs/generar-pdf/{racsId}";
        }
    }
}
