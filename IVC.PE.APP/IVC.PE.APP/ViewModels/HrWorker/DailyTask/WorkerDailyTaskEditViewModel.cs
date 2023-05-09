using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.Shared;
using IVC.PE.BINDINGRESOURCES.Areas.HrWorker;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.HrWorker.DailyTask
{
    public class WorkerDailyTaskEditViewModel : BaseViewModel
    {
        #region Phases
        private ObservableCollection<SelectItem> phases;
        public ObservableCollection<SelectItem> Phases
        {
            get { return this.phases; }
            set { this.SetValue(ref this.phases, value); }
        }
        private async void LoadPhases()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/fases-formula-cuadrilla?sgId={this.WorkerDt.SewerGroupId}");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myPhases = (List<SelectItem>)response.Result;
            this.Phases = new ObservableCollection<SelectItem>(myPhases);
        }
        #endregion
        #region SelectedPhase
        private string phase;
        public string Phase
        {
            get { return this.phase; }
            set { this.SetValue(ref this.phase, value); }
        }
        public ICommand SelectPhaseCommand => new RelayCommand(this.SelectPhase);
        private async void SelectPhase()
        {
            var searchListPopupPage = new SearchListPopupPage(this.Phases);
            await PopupNavigation.Instance.PushAsync(searchListPopupPage);
            var selection = await searchListPopupPage.PopupClosedTask;
            this.WorkerDt.ProjectPhaseId = selection.Id;
            this.Phase = selection.Text;
        }
        #endregion

        #region WorkerDailyTaskResourceModel
        private WorkerDailyTaskResourceModel workerDt;
        public WorkerDailyTaskResourceModel WorkerDt
        {
            get { return this.workerDt; }
            set { this.SetValue(ref this.workerDt, value); }
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

        #region SaveEditDailyTaskCommnad
        public ICommand SaveEditDailyTaskCommnad => new RelayCommand(this.SaveEditDailyTask);
        private async void SaveEditDailyTask()
        {
            this.IsEnabled = false;
            await PopupNavigation.Instance.PushAsync(new PopupPage());
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PutAsync(
                url,
                "/api/recursos-humanos/obreros",
                $"/tareo/editar/{this.WorkerDt.Id}",
                this.WorkerDt,
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
                    ConstantHelpers.ResponseMessages.SUCCESS_EDITED,
                    ConstantHelpers.ResponseMessages.OK
                    );

            await App.Navigator.PopAsync();
        }
        #endregion

        private readonly ApiService apiService;
        public WorkerDailyTaskEditViewModel(WorkerDailyTaskResourceModel _workerDt)
        {
            this.apiService = new ApiService();
            this.WorkerDt = _workerDt;
            this.Phase = _workerDt.Phase;
            this.IsEnabled = true;
            LoadPhases();
        }
    }
}
