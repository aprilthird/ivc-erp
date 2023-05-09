using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Extensions;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Security.Racs;
using IVC.PE.APP.Views.Shared;
using IVC.PE.BINDINGRESOURCES.Areas.Security;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.Security.Racs
{
    public class RacsViewModel : BaseViewModel
    {
        #region Projects
        private ObservableCollection<SelectItem> projects;
        public ObservableCollection<SelectItem> Projects
        {
            get { return this.projects; }
            set { this.SetValue(ref this.projects, value); }
        }
        private async void LoadProjects(Guid id)
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
            this.SelectedProject = myProjects.First(x => x.Id == id);
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
        private ObservableCollection<SelectItem> users;
        public ObservableCollection<SelectItem> Users
        {
            get { return this.users; }
            set { this.SetValue(ref this.users, value); }
        }
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
            if (!string.IsNullOrEmpty(this.Racs.ApplicationUserId))
                this.SelectedUser = this.Users.FirstOrDefault(x => x.Id.ToString().Equals(this.Racs.ApplicationUserId));
            if (!string.IsNullOrEmpty(this.Racs.LiftResponsibleId))
                this.SelectedLiftUser = this.Users.FirstOrDefault(x => x.Id.ToString().Equals(this.Racs.LiftResponsibleId));
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
            }
        }
        #endregion

        #region SelectedLiftUser
        private SelectItem selectedLiftUser;
        public SelectItem SelectedLiftUser
        {
            get { return this.selectedLiftUser; }
            set
            {
                this.SetValue(ref this.selectedLiftUser, value);
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
                "/frentes-proyecto?projectId=" + id.ToString() + (!string.IsNullOrEmpty(this.Racs.ReportDate) ? "&reportDate=" + this.Racs.ReportDate : string.Empty));

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
            if (this.Racs.WorkFrontId.HasValue)
                this.WorkFront = this.WorkFronts.First(x => x.Id == this.Racs.WorkFrontId).Text;
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
                "/cuadrillas-jefe-frente?reportDate=" + this.Racs.ReportDate + "&projectId=" + this.Racs.ProjectId.ToString() +
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
            if (this.Racs.SewerGroupId.HasValue)
                this.SewerGroup = mySewerGroups.First(x => x.Id == this.Racs.SewerGroupId.Value).Text;
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
        #endregion

        #region Observation
        private ImageSource photoStream;
        public ImageSource PhotoStream
        {
            get { return this.photoStream; }
            set { this.SetValue(ref this.photoStream, value); }
        }
        #endregion

        #region LiftPhoto
        private ImageSource liftPhotoStream;
        public ImageSource LiftPhotoStream
        {
            get { return this.liftPhotoStream; }
            set { this.SetValue(ref this.liftPhotoStream, value); }
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

        #region Racs
        private RacsResourceModel racs;
        public RacsResourceModel Racs {
            get { return this.racs; }
            set { this.SetValue(ref this.racs, value); }
        }
        private async void GetRacs(Guid racsId)
        {
            await PopupNavigation.Instance.PushAsync(new PopupPage());

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetAsync<RacsResourceModel>(
                url,
                "/api/seguridad/racs",
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

            var myRacs = (RacsResourceModel)response.Result;
            this.Racs = myRacs;
            LoadUsers(myRacs.ProjectId);
            LoadProjects(myRacs.ProjectId);
            LoadWorkFronts(myRacs.ProjectId);
            LoadSewerGroups(myRacs.WorkFrontId);
            this.SelectedDate = myRacs.ReportDate.ToDateTime();
            if (myRacs.SignatureUrl != null)
                this.SignatureStream = ImageSource.FromUri(myRacs.SignatureUrl);
            if (myRacs.ObservationImageUrl != null)
                this.PhotoStream = ImageSource.FromUri(myRacs.ObservationImageUrl);
            if (myRacs.LiftingImageUrl != null)
                this.LiftPhotoStream = ImageSource.FromUri(myRacs.LiftingImageUrl);
            return;
        }
        public void UpdateRacs(Guid racsId)
        {
            GetRacs(racsId);
        }
        #endregion

        private readonly ApiService apiService;

        public RacsViewModel(Guid racsId)
        {
            this.IsEnabled = false;
            this.apiService = new ApiService();
            this.Racs = new RacsResourceModel();
            this.SelectedDate = DateTime.Today;
            GetRacs(racsId);
        }
    }
}
