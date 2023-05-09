using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.TechnicalOffice.MixDesigns;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.MixDesigns
{
    public class MixDesignListViewModel : BaseViewModel
    {
        #region Cements
        private ObservableCollection<SelectItem> cements;
        public ObservableCollection<SelectItem> Cements
        {
            get { return this.cements; }
            set { this.SetValue(ref this.cements, value); }
        }
        private async void LoadCements()
        {
            if (this.SelectedProject == null)
            {
                this.SelectedCement = null;
                this.Cements = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/tipos-de-cemento?projectId={this.SelectedProject.Id}");

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
            this.Cements = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedCement
        private SelectItem selectedCement;
        public SelectItem SelectedCement
        {
            get { return this.selectedCement; }
            set
            {
                this.SetValue(ref this.selectedCement, value);

            }
        }
        #endregion

        //--------------

        #region DesignTypes
        private ObservableCollection<SelectItem> designTypes;
        public ObservableCollection<SelectItem> DesignTypes
        {
            get { return this.designTypes; }
            set { this.SetValue(ref this.designTypes, value); }
        }
        private async void LoadDesignTypes()
        {
            if (this.SelectedProject == null)
            {
                this.SelectedDesign = null;
                this.DesignTypes = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/diseños?projectId={this.SelectedProject.Id}");

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
            this.DesignTypes = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedDesign
        private SelectItem selectedDesign;
        public SelectItem SelectedDesign
        {
            get { return this.selectedDesign; }
            set
            {
                this.SetValue(ref this.selectedDesign, value);

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
        #region Aggs
        private ObservableCollection<SelectItem> aggs;
        public ObservableCollection<SelectItem> Aggs
        {
            get { return this.aggs; }
            set { this.SetValue(ref this.aggs, value); }
        }
        private async void LoadAggs()
        {
            if (this.SelectedProject == null)
            {
                this.SelectedAgg = null;
                this.Aggs = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/tipos-de-agregado?projectId={this.SelectedProject.Id}");

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
            this.Aggs = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedAgg
        private SelectItem selectedAgg;
        public SelectItem SelectedAgg
        {
            get { return this.selectedAgg; }
            set
            {
                this.SetValue(ref this.selectedAgg, value);

            }
        }
        #endregion
        //--------------
        #region Concrete
        private ObservableCollection<SelectItem> concretes;
        public ObservableCollection<SelectItem> Concretes
        {
            get { return this.concretes; }
            set { this.SetValue(ref this.concretes, value); }
        }
        private async void LoadConcretes()
        {
            if (this.SelectedProject == null)
            {
                this.SelectedConcrete = null;
                this.Concretes = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/uso-de-concreto?projectId={this.SelectedProject.Id}");

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
            this.Concretes = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedConcrete
        private SelectItem selectedConcrete;
        public SelectItem SelectedConcrete
        {
            get { return this.selectedConcrete; }
            set
            {
                this.SetValue(ref this.selectedConcrete, value);

            }
        }
        #endregion
        //--------------

        //--------------

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
                LoadCements();
                LoadAggs();
                LoadConcretes();
                LoadDesignTypes();
                //LoadBudgets();
                //LoadVersions();
            }

        }
        #endregion

        #region MixDesignList
        private ObservableCollection<MixDesignResourceModel> bpList;
        public ObservableCollection<MixDesignResourceModel> BPList
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

        #region LoadMixDesignsCommand
        public ICommand LoadMixDesignsCommand => new RelayCommand(this.LoadMixDesigns);
        private async void LoadMixDesigns()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<MixDesignResourceModel>(
                url,
                "/api/oficina-tecnica/diseños-de-mezcla",
                "/listar?projectId=" + this.SelectedProject.Id +
                (this.SelectedCement != null ? ("&cementTypeId=" + this.SelectedCement.Id) : string.Empty) +
                (this.SelectedAgg != null ? ("&aggTypeId=" + this.selectedAgg.Id) : string.Empty) +
                (this.SelectedConcrete != null ? ("&concreteUseId=" + this.SelectedConcrete.Id) : string.Empty) +
                (this.SelectedDesign != null ? ("&designTypeId=" + this.SelectedDesign.Id) : string.Empty) +
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

            var myBps = (List<MixDesignResourceModel>)response.Result;
            this.BPList = new ObservableCollection<MixDesignResourceModel>(myBps);

            MainViewModel.GetInstance().MixDesignListDetailViewModel = new MixDesignListDetailViewModel(this.BPList);
            await App.Navigator.PushAsync(new MixDesignListDetailPage());
        }

        #endregion




        private readonly ApiService apiService;
        public MixDesignListViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = false;
            LoadProjects();
        }
    }
}
