using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;
using static IVC.PE.APP.ViewModels.HrWorker.Attendance.WorkerAttendanceCheckViewModel;

namespace IVC.PE.APP.ViewModels.HrWorker.Attendance
{
    public class WorkerAttendanceQrCameraViewModel : BaseViewModel
    {
        #region HandleScanResultCommand
        public ICommand HandleScanResultCommand => new RelayCommand(this.HandleScanResult);
        private void HandleScanResult()
        {
            this.IsScanning = false;
            this.IsAnalyzing = false;

            Device.BeginInvokeOnMainThread(async () =>
            {
                var barcode = this.ScanResult.Text;
                if (!string.IsNullOrEmpty(barcode))
                {
                    await App.Current.MainPage.DisplayAlert("Lectura", barcode, "Ok");
                    Parent.AddAttendace(barcode);
                }
            });

            this.IsScanning = true;
            this.IsAnalyzing = true;
        }
        #endregion

        #region IsScanning
        private bool isScanning;
        public bool IsScanning
        {
            get { return this.isScanning; }
            set { this.SetValue(ref this.isScanning, value); }
        }
        #endregion

        #region IsScanning
        private bool isAnalyzing;
        public bool IsAnalyzing
        {
            get { return this.isAnalyzing; }
            set { this.SetValue(ref this.isAnalyzing, value); }
        }
        #endregion


        private readonly ApiService apiService;
        public WorkerAttendanceCheckViewModel Parent { get; set; }
        public Result ScanResult { get; set; }

        public WorkerAttendanceQrCameraViewModel(WorkerAttendanceCheckViewModel _parent)
        {
            this.apiService = new ApiService();
            this.Parent = _parent;
            this.IsScanning = true;
            this.IsAnalyzing = true;
        }
    }
}
