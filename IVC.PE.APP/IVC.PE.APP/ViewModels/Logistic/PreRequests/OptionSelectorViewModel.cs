using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Views.Logistic.PreRequests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.Logistic.PreRequests
{
    public class OptionSelectorViewModel : BaseViewModel
    {
        #region LoadListCommand
        public ICommand LoadListCommand => new RelayCommand(this.LoadLists);
        private async void LoadLists()
        {
           
            MainViewModel.GetInstance().PreRequestListViewModel = new PreRequestListViewModel();
            await App.Navigator.PushAsync(new PreRequestListPage());
        }
        #endregion

        #region LoadCreateCommand
        public ICommand LoadCreateCommand => new RelayCommand(this.LoadCreate);
        private async void LoadCreate()
        {

            MainViewModel.GetInstance().PreRequestCreateViewModel = new PreRequestCreateViewModel();
            await App.Navigator.PushAsync(new PreRequestCreatePage());
        }
        #endregion
    }
}
