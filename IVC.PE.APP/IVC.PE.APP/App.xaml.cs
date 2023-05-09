using IVC.PE.APP.Common.Extensions;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.ViewModels;
using IVC.PE.APP.ViewModels.General;
using IVC.PE.APP.Views;
using IVC.PE.APP.Views.General;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IVC.PE.APP
{
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }

        public App()
        {
            InitializeComponent();

            if (Settings.IsRemember)
            {
                var token = JsonConvert.DeserializeObject<Token>(Settings.Token);
                if (token.Expiration.ToDateTime().Date >= DateTime.Now.Date)
                {
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token = token;
                    mainViewModel.DashboardViewModel = new DashboardViewModel();
                    this.MainPage = new MasterPage();
                    return;
                }
            }

            MainViewModel.GetInstance().LoginViewModel = new LoginViewModel();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
