using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.TechnicalOffice.ErpManuals;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.ErpManuals
{
  public class ErpManualListViewModel : BaseViewModel
    {
        #region Modules
        private ObservableCollection<SelectItem> modules;
        public ObservableCollection<SelectItem> Modules
        {
            get { return this.modules; }
            set { this.SetValue(ref this.modules, value); }
        }
        private async void LoadModules()
        {



            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/modulos");

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
            this.Modules = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedModule
        private SelectItem selectedModule;
        public SelectItem SelectedModule
        {
            get { return this.selectedModule; }
            set
            {
                this.SetValue(ref this.selectedModule, value);

            }
        }
        #endregion

        #region ErpManualList
        private ObservableCollection<ErpManualResourceModel> bpList;
        public ObservableCollection<ErpManualResourceModel> BPList
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
        #region LoadErpManualsCommand
        public ICommand LoadErpManualsCommand => new RelayCommand(this.LoadErpManuals);
        private async void LoadErpManuals()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync < ErpManualResourceModel>(
                url,
                "/api/oficina-tecnica/manuales-erp",
                "/listar?" +
                (this.SelectedModule != null ? ("&versionId=" + this.SelectedModule.Id) : string.Empty)+
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

            var myBps = (List<ErpManualResourceModel>)response.Result;
            this.BPList = new ObservableCollection<ErpManualResourceModel>(myBps);

            MainViewModel.GetInstance().ErpManualListDetailViewModel = new ErpManualListDetailViewModel(this.BPList);
            await App.Navigator.PushAsync(new ErpManualListDetailPage());
        }

        #endregion

        private readonly ApiService apiService;
        public ErpManualListViewModel()
        {
            this.apiService = new ApiService();
            this.IsRefreshing = false;
            LoadModules();

        }
    }
}
