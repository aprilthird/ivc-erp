using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Shared;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.BluePrints
{
    public class BluePrintViewModel : BaseViewModel
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
                this.Bps.ProjectId = value.Id;
            }
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
        #region Bps
        private BluePrintResourceModel bps;
        public BluePrintResourceModel Bps
        {
            get { return this.bps; }
            set { this.SetValue(ref this.bps, value); }
        }
        private async void GetBps(Guid bpId)
        {
            await PopupNavigation.Instance.PushAsync(new PopupPage());

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetAsync<BluePrintResourceModel>(
                url,
                "/api/oficina-tecnica/planos",
                $"/{bpId}",
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

            var myRacs = (BluePrintResourceModel)response.Result;
            this.Bps = myRacs;
           
            LoadProjects(myRacs.ProjectId);
           

            return;
        }
        public void UpdateBps(Guid bpId)
        {
            GetBps(bpId);
        }
        #endregion


        private readonly ApiService apiService;

        public BluePrintViewModel(Guid bpId)
        {
            this.IsEnabled = false;
            this.apiService = new ApiService();
            this.Bps = new BluePrintResourceModel();
            GetBps(bpId);
        }
    }
}
