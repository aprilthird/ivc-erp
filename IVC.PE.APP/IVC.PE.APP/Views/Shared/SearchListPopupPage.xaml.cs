using IVC.PE.APP.Common.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IVC.PE.APP.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchListPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private TaskCompletionSource<SelectItem> _taskCompletionSource;
        public Task<SelectItem> PopupClosedTask => _taskCompletionSource.Task;
        public ObservableCollection<SelectItem> ItemsSource;
        public SelectItem SelectedItem;

        public SearchListPopupPage(ObservableCollection<SelectItem> _itemsSource)
        {
            this.ItemsSource = _itemsSource;
            this.SelectedItem = new SelectItem();
            InitializeComponent();
            itemList.ItemsSource = _itemsSource;
            //CloseWhenBackgroundIsClicked = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _taskCompletionSource = new TaskCompletionSource<SelectItem>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _taskCompletionSource.SetResult(this.SelectedItem);
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

        private void searchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 1)
            {
                var itemsFiltered = this.ItemsSource.Where(x => x.Text.ToUpper().Contains(e.NewTextValue.ToUpper())).ToList();
                itemList.ItemsSource = new ObservableCollection<SelectItem>(itemsFiltered.Take(20));
            } else
            {
                if (this.ItemsSource.Count > 20)
                    itemList.ItemsSource = new ObservableCollection<SelectItem>(this.ItemsSource.Take(20));
                itemList.ItemsSource = this.ItemsSource;
            }
        }

        private async void itemList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            this.SelectedItem = (SelectItem)e.SelectedItem;
            await PopupNavigation.Instance.PopAsync();
        }
    }
}