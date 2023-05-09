using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Views.Warehouse.FieldRequests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.Warehouse.FieldRequests
{
    public class FieldRequestOptionSelectorViewModel : BaseViewModel
    {
        #region LoadListCommand
        public ICommand LoadListCommand => new RelayCommand(this.LoadLists);
        private async void LoadLists()
        {

            MainViewModel.GetInstance().FieldRequestListViewModel = new FieldRequestListViewModel();
            await App.Navigator.PushAsync(new FieldRequestListPage());
        }
        #endregion

        #region LoadCreateCommand
        public ICommand LoadCreateCommand => new RelayCommand(this.LoadCreate);
        private async void LoadCreate()
        {

            MainViewModel.GetInstance().FieldRequestCreateViewModel = new FieldRequestCreateViewModel();
            await App.Navigator.PushAsync(new FieldRequestCreatePage());
        }
        #endregion
    }
}
