using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Views.TechnicalOffice.Footages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.Footages
{
    public class FootageViewModel
    {
        public ICommand SewerBoxFootagePageCommand => new RelayCommand(SewerBoxFootagePage);

        private async void SewerBoxFootagePage()
        {
            MainViewModel.GetInstance().FootageSewerBoxViewModel = new FootageSewerBoxViewModel();
            await App.Navigator.PushAsync(new FootageSewerBoxPage());
        }
    }
}
