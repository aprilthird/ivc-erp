using IVC.PE.BINDINGRESOURCES.Areas.HrWorker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IVC.PE.APP.Views.HrWorker.Attendance._Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkerQrCodeReadPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private TaskCompletionSource<List<WorkerAttendanceResourceModel>> _taskCompletionSource;
        public Task<List<WorkerAttendanceResourceModel>> PopupClosedTask => _taskCompletionSource.Task;
        public List<WorkerAttendanceResourceModel> ItemsSource;
        public List<WorkerAttendanceResourceModel> Items;
        public WorkerAttendanceResourceModel lastItem;

        public WorkerQrCodeReadPopupPage(List<WorkerAttendanceResourceModel> _itemsSource)
        {
            this.ItemsSource = _itemsSource;
            this.Items = new List<WorkerAttendanceResourceModel>();
            InitializeComponent();

            //CloseWhenBackgroundIsClicked = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            qrCode.Focus();
            _taskCompletionSource = new TaskCompletionSource<List<WorkerAttendanceResourceModel>>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _taskCompletionSource.SetResult(this.Items);
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed

            return true;
            //return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }

        private void qrCode_TextChanged(object sender, TextChangedEventArgs e)
        {            
            if (e.NewTextValue.Length > 8)
            {
                document.Text = string.Empty;
                fullName.Text = string.Empty;
                category.Text = string.Empty;
                return;
            }
            if (e.NewTextValue.Length == 8)
            {
                lastItem = ItemsSource.FirstOrDefault(x => x.Document.Equals(e.NewTextValue));
                if (lastItem != null)
                    Items.Add(lastItem);
                qrCode.Text = string.Empty;
            }
        }

        private void qrCode_Unfocused(object sender, FocusEventArgs e)
        {
            if (lastItem != null)
            {
                document.Text = lastItem.Document;
                fullName.Text = lastItem.WorkerFullName;
                category.Text = lastItem.CategoryStr;
            }
            qrCode.Focus();
        }
    }
}