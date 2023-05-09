using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.General
{
    public class LoginViewModel : BaseViewModel
    {
        #region IsRunning
        private bool isRunning;
        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }
        #endregion

        #region IsEnabled
        private bool isEnabled;
        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }
        #endregion

        #region IsRemember
        private bool isRemember;
        public bool IsRemember
        {
            get => this.isRemember;
            set => this.SetValue(ref this.isRemember, value);
        }
        #endregion

        #region IsHidden
        private bool isHidden;
        public bool IsHidden
        {
            get => this.isHidden;
            set => this.SetValue(ref this.isHidden, value);
        }
        private string hiddenIcon;
        public string HiddenIcon
        {
            get => this.hiddenIcon;
            set => this.SetValue(ref this.hiddenIcon, value);
        }
        #endregion

        #region LoginCommand
        public ICommand LoginCommand => new RelayCommand(Login);
        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Username))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ingrese un usuario.",
                    "Entendido"
                    );
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ingrese una contraseña",
                    "Entendido"
                    );
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetTokenAsync(
                url,
                "/token",
                "/crear",
                this.Username,
                this.Password
                );

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Usuario/Contraseña incorrectos.",
                    "Entendido"
                    );
                return;
            }

            var token = (Token)response.Result;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.DashboardViewModel = new DashboardViewModel();

            Settings.IsRemember = this.IsRemember;
            Settings.UserEmail = this.Username;
            Settings.UserPassword = this.Password;
            Settings.Token = JsonConvert.SerializeObject(token);

            Application.Current.MainPage = new MasterPage();
        }
        #endregion

        #region HiddenCommand
        public ICommand HiddenCommand => new RelayCommand(Hidden);
        private void Hidden()
        {
            if (this.IsHidden)
            {
                this.IsHidden = false;
                this.HiddenIcon = "\uf06e";
            } else
            {
                this.IsHidden = true;
                this.HiddenIcon = "\uf070";
            }
        }
        #endregion

        public string Username { get; set; }
        public string Password { get; set; }
        private readonly ApiService apiService;
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsRunning = false;
            this.IsEnabled = true;
            this.IsRemember = true;
            this.IsHidden = true;
            this.HiddenIcon = "\uf070";
        }

        
    }
}
