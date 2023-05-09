using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.TechnicalOffice.Procedures;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.Procedure
{
    public class ProcedureListViewModel : BaseViewModel
    {
        #region Documents
        private ObservableCollection<SelectItem> documents;
        public ObservableCollection<SelectItem> Documents
        {
            get { return this.documents; }
            set { this.SetValue(ref this.documents, value); }
        }
        private async void LoadDocuments()
        {
            if (this.SelectedProject == null)
            {
                this.SelectedDocument = null;
                this.Documents = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/tipos-de-documento?projectId={this.SelectedProject.Id}");

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
            this.Documents = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedDocument
        private SelectItem selectedDocument;
        public SelectItem SelectedDocument
        {
            get { return this.selectedDocument; }
            set
            {
                this.SetValue(ref this.selectedDocument, value);

            }
        }
        #endregion

        //--------------

        #region Processes
        private ObservableCollection<SelectItem> processes;
        public ObservableCollection<SelectItem> Processes
        {
            get { return this.processes; }
            set { this.SetValue(ref this.processes, value); }
        }
        private async void LoadProcesses()
        {

            if (this.SelectedProject == null)
            {
                this.SelectedProcess = null;
                this.Processes = new ObservableCollection<SelectItem>();
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/procesos?projectId={this.SelectedProject.Id}");

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
            this.Processes = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedProcess
        private SelectItem selectedProcess;
        public SelectItem SelectedProcess
        {
            get { return this.selectedProcess; }
            set
            {
                this.SetValue(ref this.selectedProcess, value);

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


                LoadDocuments();
                LoadProcesses();

                //LoadBudgets();
                //LoadVersions();
            }

        }
        #endregion


        #region ProviderList
        private ObservableCollection<ProcedureResourceModel> bpList;
        public ObservableCollection<ProcedureResourceModel> BPList
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

        #region LoadProceduresCommand
        public ICommand LoadProceduresCommand => new RelayCommand(this.LoadProcedures);
        private async void LoadProcedures()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<ProcedureResourceModel>(
                url,
                "/api/oficina-tecnica/procedimientos",
                "/listar?" +
                (this.SelectedDocument != null ? ("&documentTypeId=" + this.SelectedDocument.Id) : string.Empty) +
                (this.SelectedProcess != null ? ("&processId=" + this.SelectedProcess.Id) : string.Empty) +
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

            var myBps = (List<ProcedureResourceModel>)response.Result;
            this.BPList = new ObservableCollection<ProcedureResourceModel>(myBps);

            MainViewModel.GetInstance().ProcedureListDetailViewModel = new ProcedureListDetailViewModel(this.BPList);
            await App.Navigator.PushAsync(new ProcedureListDetailPage());
        }

        #endregion




        private readonly ApiService apiService;
        public ProcedureListViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = false;
            LoadProjects();

        }
    }
}
