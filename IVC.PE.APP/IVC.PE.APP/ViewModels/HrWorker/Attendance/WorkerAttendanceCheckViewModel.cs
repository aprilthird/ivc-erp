using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.HrWorker.Attendance;
using IVC.PE.APP.Views.HrWorker.Attendance._Modals;
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
using ZXing;

namespace IVC.PE.APP.ViewModels.HrWorker.Attendance
{
    public class WorkerAttendanceCheckViewModel : BaseViewModel
    {
        #region IsEnabled
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region CaptureQrCommand
        public ICommand CaptureQrCommand => new RelayCommand(this.CaptureQr);
        private async void CaptureQr()
        {
            if (this.CaptureOpt == 1)
            {
                MainViewModel.GetInstance().WorkerAttendanceQrCameraViewModel = new WorkerAttendanceQrCameraViewModel(this);
                await App.Navigator.PushAsync(new WorkerAttendanceQrCameraPage());
            }
            else if (this.CaptureOpt == 2)
            {
                var qrCodeReadPopupPage = new WorkerQrCodeReadPopupPage(this.AllWorkers);
                await PopupNavigation.Instance.PushAsync(qrCodeReadPopupPage);
                var selection = await qrCodeReadPopupPage.PopupClosedTask;
                foreach (var item in selection)
                {
                    item.Attended = true;
                    item.AttendedIcon = "\uf00c";
                }
                this.Attendances.AddRange(selection);
                this.Workers = new ObservableCollection<WorkerAttendanceResourceModel>(this.Attendances);
            }
        }
        #endregion

        #region Workers
        private ObservableCollection<WorkerAttendanceResourceModel> workers;
        public ObservableCollection<WorkerAttendanceResourceModel> Workers
        {
            get { return this.workers; }
            set { this.SetValue(ref this.workers, value); }
        }
        private async void LoadWorkers()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<WorkerAttendanceResourceModel>(
                url,
                "/api/recursos-humanos/obreros",
                $"/asistencia/obreros-habiles?taskDate={this.TaskDate}&sewerGroupId={this.SewerGroupId}&projectId={this.ProjectId}",
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

            var myWorkers = (List<WorkerAttendanceResourceModel>)response.Result;
            this.AllWorkers = myWorkers;
        }
        #endregion

        #region SaveAttendanceCommand
        public ICommand SaveAttendanceCommand => new RelayCommand(this.SaveAttendance);
        private async void SaveAttendance()
        {
            this.IsEnabled = false;
            await PopupNavigation.Instance.PushAsync(new PopupPage());

            var attendacesToSave = new WorkerAttendanceListResourceModel
            {
                ProjectId = this.ProjectId,
                SewerGroupId = this.SewerGroupId,
                TaskDate = this.TaskDate,
                WorkerAttendances = this.Attendances
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostAsync(
                url,
                "/api/recursos-humanos/obreros",
                "/asistencia/registrar",
                attendacesToSave,
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

            await App.Navigator.PopAsync();
        }
        #endregion

        private readonly ApiService apiService;
        public int CaptureOpt { get; set; }
        public string TaskDate { get; set; }
        public Guid SewerGroupId { get; set; }
        public Guid ProjectId { get; set; }
        public List<WorkerAttendanceResourceModel> AllWorkers;
        public List<WorkerAttendanceResourceModel> Attendances;

        public WorkerAttendanceCheckViewModel(int _captureOpt, string _taskDate, Guid _sewerGroupId, Guid _projectId)
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.CaptureOpt = _captureOpt;
            this.TaskDate = _taskDate;
            this.SewerGroupId = _sewerGroupId;
            this.ProjectId = _projectId;
            this.Workers = new ObservableCollection<WorkerAttendanceResourceModel>();
            this.Attendances = new List<WorkerAttendanceResourceModel>();
            this.AllWorkers = new List<WorkerAttendanceResourceModel>();
            LoadWorkers();
        }

        public void AddAttendace(string _code)
        {
            if(!string.IsNullOrEmpty(_code))
            {
                var worker = this.AllWorkers.FirstOrDefault(x => x.Document == _code);
                if (worker != null)
                {
                    if (!worker.Attended)
                    {
                        worker.Attended = true;
                        worker.AttendedIcon = "\uf00c";
                        this.Attendances.Add(worker);
                        Workers = new ObservableCollection<WorkerAttendanceResourceModel>(this.Attendances);
                    }
                }
            }
        }
    }
}
