using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Security.Racs._Modals;
using IVC.PE.APP.Views.Shared;
using IVC.PE.BINDINGRESOURCES.Areas.Security;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Security.Racs
{
    public class RacsLiftViewModel : BaseViewModel
    {
        #region Observation
        private ImageSource photoStream;
        public ImageSource PhotoStream
        {
            get { return this.photoStream; }
            set { this.SetValue(ref this.photoStream, value); }
        }
        #endregion

        #region AddLiftPhotoCommand
        private ImageSource liftPhotoStream;
        public ImageSource LiftPhotoStream
        {
            get { return this.liftPhotoStream; }
            set { this.SetValue(ref this.liftPhotoStream, value); }
        }
        public ICommand AddLiftPhotoCommand => new RelayCommand(this.AddLiftPhoto);
        private async void AddLiftPhoto()
        {
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("¿De dónde desea obtener la foto?", "Cancelar", "",
                new string[] { "Cámara", "Galería" });

            switch (selectedOption)
            {
                case "Cámara":
                    ObservationImage = await TakePictureFromCamera();
                    break;
                case "Galería":
                    ObservationImage = await TakePictureFromLibrary();
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(ObservationImage))
            {
                //await App.Navigator.PushAsync(new RacsPhotoEditorModalPage(ObservationImage, this));
                await App.Current.MainPage.Navigation.PushModalAsync(new RacsLiftPhotoEditoModalPage(ObservationImage, this), true);
            }
        }
        #endregion

        #region SaveRacsLiftCommand
        public ICommand SaveRacsLiftCommand => new RelayCommand(this.SaveRacsLift);
        private async void SaveRacsLift()
        {
            var result = await App.Current.MainPage.DisplayAlert(
                "Confirmar acción",
                "Desea grabar el levantamiento del RACS.",
                "Sí, grabar.",
                "No");

            if (result)
            {
                await PopupNavigation.Instance.PushAsync(new PopupPage());

                var url = Application.Current.Resources["UrlAPI"].ToString();
                var response = await this.apiService.PutAsync<RacsToLift>(
                    url,
                    "/api/seguridad/racs",
                    "/levantar-observacion",
                    this.RacsToLift,
                    "bearer",
                    MainViewModel.GetInstance().Token.Access_Token
                    );

                await PopupNavigation.Instance.PopAsync();

                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        ConstantHelpers.ResponseMessages.FAIL,
                        response.Message,
                        ConstantHelpers.ResponseMessages.OK
                        );
                    return;
                }

                await Application.Current.MainPage.DisplayAlert(
                        ConstantHelpers.ResponseMessages.SUCCESS,
                        ConstantHelpers.ResponseMessages.SUCCESS_ADDED,
                        ConstantHelpers.ResponseMessages.OK
                        );


                this.Parent.UpdateRacs();
                await App.Navigator.PopAsync();
            }

            return;
        }
        #endregion

        #region RacsToLift
        private RacsToLift racsToLift;
        public RacsToLift RacsToLift
        {
            get { return this.racsToLift; }
            set { this.SetValue(ref this.racsToLift, value); }
        }
        #endregion

        private readonly ApiService apiService;
        public RacsListViewModel Parent { get; set; }
        public string ObservationImage { get; set; }
        public bool IsBusy { get; set; }
        public RacsLiftViewModel(Guid racsId, RacsListViewModel _parent)
        {
            this.apiService = new ApiService();
            this.RacsToLift = new RacsToLift
            {
                Id = racsId
            };
            this.Parent = _parent;
            GetRacsToLift(racsId);
        }

        private async void GetRacsToLift(Guid racsId)
        {
            await PopupNavigation.Instance.PushAsync(new PopupPage());

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetAsync<RacsToLift>(
                url,
                "/api/seguridad/racs/levantar-observacion",
                $"/{racsId}",
                "bearer",
                MainViewModel.GetInstance().Token.Access_Token
                );

            await PopupNavigation.Instance.PopAsync();

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    ConstantHelpers.ResponseMessages.FAIL,
                    response.Message,
                    ConstantHelpers.ResponseMessages.OK
                    );
                return;
            }

            var myRacs = (RacsToLift)response.Result;
            this.RacsToLift = myRacs;
            if (myRacs.ObservationImageUrl != null)
                this.PhotoStream = ImageSource.FromUri(myRacs.ObservationImageUrl);
            return;
        }

        #region Helpers
        public void UpdateLiftPhotoStream(MemoryStream _stream)
        {
            this.RacsToLift.LiftingImageArray = _stream.ToArray();
            this.LiftPhotoStream = ImageSource.FromStream(() => { return _stream; });
        }
        private async Task<string> TakePictureFromLibrary()
        {
            IsBusy = true;
            var file = await CrossMedia.Current.PickPhotoAsync
                (new PickMediaOptions()
                {
                    SaveMetaData = true,
                    PhotoSize = PhotoSize.MaxWidthHeight
                });
            IsBusy = false;
            if (file == null)
                return null;

            return file.Path;

        }
        private async Task<string> TakePictureFromCamera()
        {
            IsBusy = true;
            var file = await CrossMedia.Current.TakePhotoAsync
                (new StoreCameraMediaOptions()
                {
                    SaveMetaData = true,
                    PhotoSize = PhotoSize.MaxWidthHeight
                });

            IsBusy = false;
            if (file == null)
                return null;

            return file.Path;
        }
        #endregion
    }
}
