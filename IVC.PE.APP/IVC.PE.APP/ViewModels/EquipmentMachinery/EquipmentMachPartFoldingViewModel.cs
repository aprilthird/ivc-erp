using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.APP.Views.EquipmentMachinery;
using IVC.PE.APP.Views.EquipmentMachinery._Modals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.EquipmentMachinery
{
   public class EquipmentMachPartFoldingViewModel : BaseViewModel
    {

        #region Operators
        public Guid SelectedOperatorId { get; set; }

        public string OperatorName { get; set; }

        #endregion

        //#region Operators
        //private ObservableCollection<SelectItem> operators;
        //public ObservableCollection<SelectItem> Operators
        //{
        //    get { return this.operators; }
        //    set { this.SetValue(ref this.operators, value); }
        //}
        //private async void LoadOperators()
        //{

        //    var url = Application.Current.Resources["UrlAPI"].ToString();
        //    var response = await this.apiService.GetListAsync<SelectItem>(
        //        url,
        //        "/select",
        //        $"/operadores-maquinaria");

        //    if (!response.IsSuccess)
        //    {
        //        await Application.Current.MainPage.DisplayAlert(
        //            "Error",
        //            response.Message,
        //            "Entendido"
        //            );
        //        return;
        //    }

        //    var myWorkFrontHeads = (List<SelectItem>)response.Result;
        //    this.Operators = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        //}
        //#endregion
        //#region SelectedOperator
        //private SelectItem selectedOperator;
        //public SelectItem SelectedOperator
        //{
        //    get { return this.selectedOperator; }
        //    set
        //    {
        //        this.SetValue(ref this.selectedOperator, value);

        //    }
        //}
        //#endregion


        //--------------

        #region SewerGroups
        private ObservableCollection<SelectItem> sewerGroups;
        public ObservableCollection<SelectItem> SewerGroups
        {
            get { return this.sewerGroups; }
            set { this.SetValue(ref this.sewerGroups, value); }
        }
        private async void LoadSewerGroups()
        {
            //--PROJECTID
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/cuadrillas-maquinarias?projectId="+Guid.Parse("CB9CD712-E2DB-421A-52F0-08D88325D938"));

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myWorkFrontHeads = (List<SelectItem>)response.Result;
            this.SewerGroups = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedSewerGroup
        private SelectItem selectedSewerGroup;
        public SelectItem SelectedSewerGroup
        {
            get { return this.selectedSewerGroup; }
            set
            {
                this.SetValue(ref this.selectedSewerGroup, value);

            }
        }
        #endregion

        //--------------


        #region Users
        private ObservableCollection<SelectItem> users;
        public ObservableCollection<SelectItem> Users
        {
            get { return this.users; }
            set { this.SetValue(ref this.users, value); }
        }
        private async void LoadUsers()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/empleados");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myWorkFrontHeads = (List<SelectItem>)response.Result;
            this.Users = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedUser
        private SelectItem selectedUser;
        public SelectItem SelectedUser
        {
            get { return this.selectedUser; }
            set
            {
                this.SetValue(ref this.selectedUser, value);

            }
        }
        #endregion

        //--------------

        

        //--------------
public string QrCode { get; set; }

        public int Option { get; set; }
        //--------------
        private string partNumber;
        public string PartNumber
        {
            get { return this.partNumber; }
            set
            {
                this.SetValue(ref this.partNumber, value);
                OnPropertyChanged();
            }
        }

        //---------------

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        //--------------

        #region LoadBluePrintsCommand
        public ICommand ToFoldingVariableCommand => new RelayCommand(this.LoadFoldings);
        private async void LoadFoldings()
        {





            MainViewModel.GetInstance().EquipmentMachPartFoldingVariableViewModel = new EquipmentMachPartFoldingVariableViewModel(this.QrCode, this.SelectedUser.Id.ToString(), this.SelectedOperatorId, this.SelectedSewerGroup.Id, this.PartNumber);
            await App.Navigator.PushAsync(new EquipmentMachPartFoldingVariablePage());
        }

        #endregion

        //--------------
        private readonly ApiService apiService;
        public EquipmentMachPartFoldingViewModel([Optional] string qrcode, [Optional] Guid SelectedOperatorId, [Optional] string OperatorName)
        {
            this.apiService = new ApiService();
            this.QrCode = qrcode;
            this.SelectedOperatorId = SelectedOperatorId;
            this.OperatorName = OperatorName;

            LoadUsers();
            LoadSewerGroups();
                 
        }



    }
}
