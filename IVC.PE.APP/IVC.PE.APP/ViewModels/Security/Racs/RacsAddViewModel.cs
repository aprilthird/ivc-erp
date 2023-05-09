using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Security.Racs._Modals;
using IVC.PE.APP.Views.Shared;
using IVC.PE.BINDINGRESOURCES.Areas.Security;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Security.Racs
{
    public class RacsAddViewModel : BaseViewModel
    {
        #region Projects
        private ObservableCollection<SelectItem> projects;
        public ObservableCollection<SelectItem> Projects
        {
            get { return this.projects; }
            set { this.SetValue(ref this.projects, value); }
        }
        private async void LoadProjects()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                "/proyectos");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myProjects = (List<SelectItem>)response.Result;
            this.Projects = new ObservableCollection<SelectItem>(myProjects);
            this.SelectedProject = myProjects.FirstOrDefault();
        }
        #endregion

        #region SelectedProject
        private SelectItem selectedProject;
        public SelectItem SelectedProject
        {
            get { return this.selectedProject; }
            set
            {
                this.SetValue(ref this.selectedProject, value);
                this.Racs.ProjectId = value.Id;
                LoadWorkFronts(value.Id);
                LoadUsers(value.Id);
                //LoadSewerGroups();
            }
        }
        #endregion

        #region SelectedDate
        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return this.selectedDate; }
            set
            {
                this.SetValue(ref this.selectedDate, value);
                this.Racs.ReportDate = value.ToString("dd/MM/yyyy");
            }
        }
        #endregion

        #region Users
        private string liftUser;
        public string LiftUser
        {
            get { return this.liftUser; }
            set
            {
                this.SetValue(ref this.liftUser, value);
            }
        }
        public ObservableCollection<SelectItem> Users { get; set; }
        private async void LoadUsers(Guid pId)
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/usuarios-proyecto?projectId={pId}");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myUsers = (List<SelectItem>)response.Result;
            this.Users = new ObservableCollection<SelectItem>(myUsers.OrderBy(x => x.Text));
        }
        public ICommand SelectLiftUserCommand => new RelayCommand(this.SelectLiftUser);
        private async void SelectLiftUser()
        {
            var searchListPopupPage = new SearchListPopupPage(this.Users);
            await PopupNavigation.Instance.PushAsync(searchListPopupPage);
            var selection = await searchListPopupPage.PopupClosedTask;
            this.Racs.LiftResponsibleId = selection.Id.ToString();
            this.LiftUser = selection.Text;
        }
        #endregion

        #region SelectedUser
        private SelectItem selectedUser;
        public SelectItem SelectedUser
        {
            get { return this.selectedUser; }
            set
            {
                this.SetValue(ref this.selectedUser, value);
                this.Racs.LiftResponsibleId = value.Id.ToString();
            }
        }
        #endregion

        #region WorkFronts
        private string workFront;
        public string WorkFront
        {
            get { return this.workFront; }
            set 
            {
                this.SetValue(ref this.workFront, value);
            }
        }
        public ObservableCollection<SelectItem> WorkFronts { get; set; }
        private async void LoadWorkFronts(Guid id)
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                "/frentes-proyecto?projectId=" + id.ToString());

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myWorkFronts = (List<SelectItem>)response.Result;
            this.WorkFronts = new ObservableCollection<SelectItem>(myWorkFronts.OrderBy(x => x.Text));
        }
        public ICommand SelectWorkFrontCommand => new RelayCommand(this.SelectWorkFront);
        private async void SelectWorkFront()
        {
            var searchListPopupPage = new SearchListPopupPage(this.WorkFronts);
            await PopupNavigation.Instance.PushAsync(searchListPopupPage);
            var selection = await searchListPopupPage.PopupClosedTask;
            this.Racs.WorkFrontId = selection.Id;
            LoadSewerGroups(selection.Id);
            this.WorkFront = selection.Text;
        }
        #endregion

        #region SewerGroups
        private string sewerGroup;
        public string SewerGroup
        {
            get { return this.sewerGroup; }
            set
            {
                this.SetValue(ref this.sewerGroup, value);
            }
        }
        public ObservableCollection<SelectItem> SewerGroups { get; set; }
        private async void LoadSewerGroups(Guid? id = null)
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                "/cuadrillas-frente?&projectId=" + this.SelectedProject.Id.ToString() +
                        (id.HasValue ? "&workFrontId=" + id.ToString() : string.Empty));

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var mySewerGroups = (List<SelectItem>)response.Result;
            this.SewerGroups = new ObservableCollection<SelectItem>(mySewerGroups.OrderBy(x => x.Text));
        }
        public ICommand SelectSewerGroupCommand => new RelayCommand(this.SelectSewerGroup);
        private async void SelectSewerGroup()
        {
            var searchListPopupPage = new SearchListPopupPage(this.SewerGroups);
            await PopupNavigation.Instance.PushAsync(searchListPopupPage);
            var selection = await searchListPopupPage.PopupClosedTask;
            this.Racs.SewerGroupId = selection.Id;
            this.SewerGroup = selection.Text;
        }
        #endregion

        #region SelectedSewerGroup
        private SelectItem selectedSewerGroup;
        public SelectItem SelectedSewerGroup
        {
            get { return this.selectedSewerGroup; }
            set
            {
                this.SetValue(ref this.selectedSewerGroup, value);
                this.Racs.SewerGroupId = value.Id;
            }
        }
        #endregion

        #region Signature
        private ImageSource signatureStream;
        public ImageSource SignatureStream
        {
            get { return this.signatureStream; }
            set { this.SetValue(ref this.signatureStream, value); }
        }
        public ICommand AddSignatureCommand => new RelayCommand(this.AddSignature);
        private async void AddSignature()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new RacsSignatureModalPage(this), true);
        }
        #endregion

        #region Location
        //private ImageSource locationStream;
        //public ImageSource LocationStream
        //{
        //    get { return this.locationStream; }
        //    set { this.SetValue(ref this.locationStream, value); }
        //}
        //public ICommand AddLocationCommand => new RelayCommand(this.AddLocation);
        //private async void AddLocation()
        //{
        //    await App.Current.MainPage.Navigation.PushModalAsync(new RacsMapLocationModalPage(this), true);
        //}
        #endregion

        #region Observation
        private ImageSource photoStream;
        public ImageSource PhotoStream
        {
            get { return this.photoStream; }
            set { this.SetValue(ref this.photoStream, value); }
        }
        public ICommand AddPhotoCommand => new RelayCommand(this.AddPhoto);
        private async void AddPhoto()
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
                await App.Current.MainPage.Navigation.PushModalAsync(new RacsPhotoEditorModalPage(ObservationImage, this), true);
            }
        }
        #endregion

        #region SaveRacsCommand
        public ICommand SaveRacsCommand => new RelayCommand(this.SaveRacs);
        private async void SaveRacs()
        {
            this.IsEnabled = false;
            this.Racs.ApplicationUserId = MainViewModel.GetInstance().Token.UserId;
            await PopupNavigation.Instance.PushAsync(new PopupPage());
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostAsync<RacsResourceModel>(
                url,
                "/api/seguridad/racs",
                "/crear",
                this.Racs,
                "bearer",
                MainViewModel.GetInstance().Token.Access_Token
                );

            this.IsEnabled = true;
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
        #endregion

        #region IsEnabled
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        #endregion

        private readonly ApiService apiService;
        public RacsListViewModel Parent { get; set; }
        public RacsResourceModel Racs { get; set; }
        public string ObservationImage { get; set; }

        public RacsAddViewModel(RacsListViewModel _parent)
        {
            this.apiService = new ApiService();
            this.Parent = _parent;
            this.Racs = new RacsResourceModel();
            this.SelectedDate = DateTime.Today;
            this.IsEnabled = true;
            LoadProjects();
            //LoadWorkFronts();
            //LoadSewerGroups();
            //LoadUsers();
        }

        #region Helpers
        public void UpdateSignatureStream(MemoryStream _stream)
        {
            this.Racs.SignatureArray = _stream.ToArray();
            this.SignatureStream = ImageSource.FromStream(() => { return _stream; });
        }

        public void UpdateLocationStream(MemoryStream _stream)
        {
            //this.Racs.LocationImageArray = _stream.ToArray();
            //this.LocationStream = ImageSource.FromStream(() => { return _stream; });
        }

        public void UpdatePhotoStream(MemoryStream _stream)
        {
            this.Racs.ObservationImageArray = _stream.ToArray();
            this.PhotoStream = ImageSource.FromStream(() => { return _stream; });
        }

        private async Task<string> TakePictureFromLibrary()
        {
            var file = await CrossMedia.Current.PickPhotoAsync
                (new PickMediaOptions()
                {
                    SaveMetaData = true,
                    PhotoSize = PhotoSize.MaxWidthHeight
                });

            if (file == null)
                return null;

            return file.Path;

        }
        private async Task<string> TakePictureFromCamera()
        {
            var file = await CrossMedia.Current.TakePhotoAsync
                (new StoreCameraMediaOptions()
                {
                    SaveMetaData = true,
                    PhotoSize = PhotoSize.MaxWidthHeight
                });

            if (file == null)
                return null;

            return file.Path;
        }
        #endregion
    }
}
