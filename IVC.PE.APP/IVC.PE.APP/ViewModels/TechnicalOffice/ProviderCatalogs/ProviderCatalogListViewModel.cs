using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.TechincalOffice.ProviderCatalogs;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.ProviderCatalogs
{
    public class ProviderCatalogListViewModel : BaseViewModel
    {
        #region Groups
        private ObservableCollection<SelectItem> groups;
        public ObservableCollection<SelectItem> Groups
        {
            get { return this.groups; }
            set { this.SetValue(ref this.groups, value); }
        }
        private async void LoadGroups()
        {
            if (this.SelectedFamily == null)
            {
                this.SelectedGroup = null;
                this.Groups = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/grupos-de-insumos-familia?familyId={this.SelectedFamily.Id}");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myWorkFrontHeads = (List<SelectItem>)response.Result;
            this.Groups = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedGroup
        private SelectItem selectedGroup;
        public SelectItem SelectedGroup
        {
            get { return this.selectedGroup; }
            set
            {
                this.SetValue(ref this.selectedGroup, value);

            }
        }
        #endregion

        //--------------

        #region Families
        private ObservableCollection<SelectItem> families;
        public ObservableCollection<SelectItem> Families
        {
            get { return this.families; }
            set { this.SetValue(ref this.families, value); }
        }
        private async void LoadFamilies()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/familias-de-insumos");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myWorkFrontHeads = (List<SelectItem>)response.Result;
            this.Families = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedFamily
        private SelectItem selectedFamily;
        public SelectItem SelectedFamily
        {
            get { return this.selectedFamily; }
            set
            {
                this.SetValue(ref this.selectedFamily, value);
                LoadGroups();
            }
        }
        #endregion
        //--------------
        private string name;

        public string Name
        {
            get { return this.name; }
            set
            {
                this.SetValue(ref this.name, value);
                OnPropertyChanged();
            }

        }
        //--
        //--------------
        #region Specs
        private ObservableCollection<SelectItem> specs;
        public ObservableCollection<SelectItem> Specs
        {
            get { return this.specs; }
            set { this.SetValue(ref this.specs, value); }
        }
        private async void LoadSpecs()
        {
            if (this.SelectedProject == null)
            {
                this.SelectedSpec = null;
                this.Specs = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/especialidades?projectId={this.SelectedProject.Id}");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myWorkFrontHeads = (List<SelectItem>)response.Result;
            this.Specs = new ObservableCollection<SelectItem>(myWorkFrontHeads);
            //this.SelectedSpec = myWorkFrontHeads.FirstOrDefault();
        }
        #endregion
        #region SelectedSpec
        private SelectItem selectedSpec;
        public SelectItem SelectedSpec
        {
            get { return this.selectedSpec; }
            set
            {
                this.SetValue(ref this.selectedSpec, value);

            }
        }
        #endregion

        //--------------
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


                LoadSpecs();
                
                //LoadBudgets();
                //LoadVersions();
            }

        }
        #endregion

        #region ProviderList
        private ObservableCollection<ProviderCatalogResourceModel> bpList;
        public ObservableCollection<ProviderCatalogResourceModel> BPList
        {
            get { return this.bpList; }
            set { this.SetValue(ref this.bpList, value); }
        }
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region LoadProvidersCommand
        public ICommand LoadProvidersCommand => new RelayCommand(this.LoadProviders);
        private async void LoadProviders()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<ProviderCatalogResourceModel>(
                url,
                "/api/oficina-tecnica/catalogo-de-proveedores",
                "/listar?"+
                (this.SelectedFamily != null ? ("&familyId=" + this.SelectedFamily.Id) : string.Empty) +
                (this.SelectedGroup != null ? ("&groupId=" + this.SelectedGroup.Id) : string.Empty) +
                (this.SelectedSpec != null ? ("&specId=" + this.SelectedSpec.Id) : string.Empty) +
                (this.Name != null ? ("&str=" + this.Name) : string.Empty),
                "bearer",
                MainViewModel.GetInstance().Token.Access_Token);

            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myBps = (List<ProviderCatalogResourceModel>)response.Result;
            this.BPList = new ObservableCollection<ProviderCatalogResourceModel>(myBps);

            MainViewModel.GetInstance().ProviderCatalogListDetailViewModel = new ProviderCatalogListDetailViewModel(this.BPList);
            await App.Navigator.PushAsync(new ProviderCatalogListDetailPage());
        }

        #endregion




        private readonly ApiService apiService;
        public ProviderCatalogListViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = false;
            LoadProjects();
            LoadFamilies();
        }
    }
}
