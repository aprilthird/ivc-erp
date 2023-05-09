using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.BluePrints
{
   public class BluePrintQrCameraViewModel : BaseViewModel
    {
        #region HandleScanResultCommand
        public ICommand HandleScanResultCommand => new RelayCommand(this.HandleScanResult);
        private void HandleScanResult()
        {
            this.IsScanning = false;
            this.IsAnalyzing = false;

            Device.BeginInvokeOnMainThread(async () =>
            {
                var barcode = this.ScanResult.Text.Split('/');
                string bpCode = barcode[0].ToString();

                string bpVer = barcode[1].ToString();
                Parent.UpdateCode(bpCode,bpVer);
                if (!string.IsNullOrEmpty(bpCode))
                {
                    await App.Current.MainPage.DisplayAlert("Lectura", bpCode+"-"+bpVer, "Ok");
                    var url = Application.Current.Resources["UrlAPI"].ToString();

                    var response = await this.apiService.GetListAsync<BluePrintUspResourceModel>(
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
                    this.AllBluePrints = myPdfUrl;
                    Uri tempUri;
                    //Version Aprobada
                    //Ver Plano (Opcion)
                    //Ver Carta (Opcion)
                    if (bpVer.Equals("Aprobada"))
                    {
                        string selectedOption = await App.Current.MainPage.DisplayActionSheet("Version Aprobada", "Cancelar", "",
                                    new string[] { "Plano Aprobado", "Carta" });
                        //this.MyFileUrl = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();

                        switch (selectedOption)
                        {
                            //ver plano aprobado
                            //ver carta
                            case "Plano Aprobado":
                                tempUri = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();
                                this.MyFileUrl = tempUri;
                                break;
                            case "Carta":
                                
                                tempUri = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.LetterUrl).FirstOrDefault();
                                this.MyFileUrl = tempUri;
                                break;
                            default:
                                break;
                        }

                    }
                    else if (bpVer.Equals("Contractual") && AllBluePrints.Count() == 1)
                    {
                        string selectedOption = await App.Current.MainPage.DisplayActionSheet("Version Contractual, Este Plano aun no tiene una Version Aprobada", "Cancelar", "",
                                    new string[] { "Ver"});
                        //this.MyFileUrl = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();

                        switch (selectedOption)
                        {
                            //ver plano aprobado
                            //ver carta
                            case "Ver":
                                tempUri = this.AllBluePrints.Where(x => x.Description == "Contractual").Select(x => x.FileUrl).FirstOrDefault();
                                this.MyFileUrl = tempUri;
                                break;
                            default:
                                break;
                        }
                        


                        //Version Contractual, Este Plano aun no tiene una Version Aprobada
                        //, OK , Visualizar



                    }
                    else if (bpVer.Equals("Contractual") && AllBluePrints.Count() == 2)
                    {
                        foreach (var item in this.AllBluePrints)
                        {
                            if (item.Description == "Aprobada")
                            {
                                //agregar carta
                                string selectedOption = await App.Current.MainPage.DisplayActionSheet("Version Contractual, Existe una versión aprobada", "Cancelar", "",
                                    new string[] { "Plano Aprobado", "Carta" ,"Plano Contractual"});

                                switch (selectedOption)
                                {
                                    //ver plano aprobado
                                    //ver carta
                                    case "Plano Aprobado":
                                        tempUri = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.FileUrl).FirstOrDefault();
                                        this.MyFileUrl = tempUri;
                                        break;
                                    case "Carta":
                                        tempUri = this.AllBluePrints.Where(x => x.Description == "Aprobada").Select(x => x.LetterUrl).FirstOrDefault();
                                        this.MyFileUrl = tempUri;
                                        break;
                                    case "Plano Contractual":
                                        tempUri = this.AllBluePrints.Where(x => x.Description == "Contractual").Select(x => x.FileUrl).FirstOrDefault(); ;
                                        this.MyFileUrl = tempUri;
                                        break;
                                }
                            }
                        }


                    }
                    Parent.UpdateWebView(this.MyFileUrl);
                    await App.Navigator.PopAsync();
                }
            });

            this.IsScanning = true;
            this.IsAnalyzing = true;
        }
        #endregion

        #region IsScanning
        private bool isScanning;
        public bool IsScanning
        {
            get { return this.isScanning; }
            set { this.SetValue(ref this.isScanning, value); }
        }
        #endregion

        #region IsScanning
        private bool isAnalyzing;
        public bool IsAnalyzing
        {
            get { return this.isAnalyzing; }
            set { this.SetValue(ref this.isAnalyzing, value); }
        }
        #endregion


        private readonly ApiService apiService;
        public BluePrintCheckViewModel Parent { get; set; }
        public Result ScanResult { get; set; }

        public Uri MyFileUrl { get; set; }

        public List<BluePrintUspResourceModel> AllBluePrints; 
        public BluePrintQrCameraViewModel(BluePrintCheckViewModel _parent)
        {
            this.apiService = new ApiService();
            this.Parent = _parent;
            this.IsScanning = true;
            this.IsAnalyzing = true;
            this.AllBluePrints = new List<BluePrintUspResourceModel>();
        }
    }
}
