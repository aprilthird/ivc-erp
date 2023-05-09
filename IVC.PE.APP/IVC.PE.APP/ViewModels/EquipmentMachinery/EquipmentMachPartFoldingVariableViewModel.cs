using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.BINDINGRESOURCES.Areas.EquipmentMachinery;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.EquipmentMachinery
{
   public class EquipmentMachPartFoldingVariableViewModel : BaseViewModel
    {

        #region Phases
        private ObservableCollection<SelectItem> phases;
        public ObservableCollection<SelectItem> Phases
        {
            get { return this.phases; }
            set { this.SetValue(ref this.phases, value); }
        }
        private async void LoadPhases()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/fases-proyecto-maquinaria-app?projectid=CB9CD712-E2DB-421A-52F0-08D88325D938");

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
            this.Phases = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedOperator
        private SelectItem selectedPhase;
        public SelectItem SelectedPhase
        {
            get { return this.selectedPhase; }
            set
            {
                this.SetValue(ref this.selectedPhase, value);

            }
        }
        #endregion
        //--
        public string SelectedUserId { get; set; }

        public string PartNumber { get; set; }

        public Guid SelectedOperatorId { get; set; }

        public Guid SelectedSewerGroupId { get; set; }


        //--

       
        //--


        //--
        #region SelectedMachine
        private Guid selectedMachineId;
        public Guid SelectedMachineId
        {
            get { return this.selectedMachineId; }
            set
            {
                this.SetValue(ref this.selectedMachineId, value);
                LoadActs();
            }
        }
        #endregion
        private async void LoadMachines()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<EquipmentMachPartConsultingResourceModel>(
                url,
                "/api/equipos/parte-equipos-maquinaria",
                "/consultar-qr?qrString=" + this.QrCode +
                "&month=" + DateTime.UtcNow.Date.Month +
                "&year=" + DateTime.UtcNow.Date.Year,
                "bearer",
                MainViewModel.GetInstance().Token.Access_Token);
            var list = (List<EquipmentMachPartConsultingResourceModel>)response.Result;

            var machine = list.FirstOrDefault();
            this.SelectedMachineId = machine.EquipmentMachineryTypeTypeId;
        }


        public string QrCode { get; set; }
     



        //--
        private double initHorometer;
        public double InitHorometer
        {
            get { return this.initHorometer; }
            set
            {
                this.SetValue(ref this.initHorometer, value);
                OnPropertyChanged();
            }
        }
        //--

        #region Acts
        private ObservableCollection<SelectItem> activities;
        public ObservableCollection<SelectItem> Activities
        {
            get { return this.activities; }
            set { this.SetValue(ref this.activities, value); }
        }
        private async void LoadActs()
        {

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItem>(
                url,
                "/select",
                $"/actividad-maquinaria-seleccionado-folding?tsId="+this.SelectedMachineId);

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
            this.Activities = new ObservableCollection<SelectItem>(myWorkFrontHeads);
        }
        #endregion
        #region SelectedActivitiy
        private SelectItem selectedActivity;
        public SelectItem SelectedActivity
        {
            get { return this.selectedActivity; }
            set
            {
                this.SetValue(ref this.selectedActivity, value);

            }
        }
        #endregion
        //--
        private double endHorometer;
        public double EndHorometer
        {
            get { return this.endHorometer; }
            set
            {
                this.SetValue(ref this.endHorometer, value);
                OnPropertyChanged();
            }
        }
        //--
        #region SelectedDate
        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return this.selectedDate; }
            set
            {
                this.SetValue(ref this.selectedDate, value);
            }
        }
        #endregion

        private EquipmentMachPartResourceModel list;

        public EquipmentMachPartResourceModel List
        {
            get { return this.list; }
            set { this.SetValue(ref this.list, value); }
        }



        #region LoadBluePrintsCommand
        public ICommand ToSaveCommand => new RelayCommand(this.LoadBluePrints);
        private async void LoadBluePrints()
        {

            var toSave = new EquipmentMachPartFoldingResourceModel
            {



                PartNumber = this.PartNumber,
        PartDate = this.selectedDate,

        EquipmentMachineryOperatorId = this.SelectedOperatorId,

        UserId = this.SelectedUserId,
        
        InitHorometer = this.InitHorometer,

        EndHorometer = this.EndHorometer,

        EquipmentMachineryTypeTypeActivityId = this.SelectedActivity.Id,

        SewerGroupId = this.SelectedSewerGroupId,


        MachineryPhaseId = this.SelectedPhase.Id    
            };

            var url2 = Application.Current.Resources["UrlAPI"].ToString();
            var response2 = await this.apiService.PostAsync(
                url2,
                "/api/equipos/parte-equipos-maquinaria",
                "/parte-diario/registrar?qrString=" +this.QrCode,
                toSave,
                "bearer",
                MainViewModel.GetInstance().Token.Access_Token
                );


            if (!response2.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    ConstantHelpers.ResponseMessages.FAIL,
                    response2.Message,
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

            this.SelectedActivity = null;
            InitHorometer = 0;
            EndHorometer = 0;

            var url3 = Application.Current.Resources["UrlAPI"].ToString();
            var response3 = await this.apiService.PutAsync<EquipmentMachPartResourceModel>(
                url3,
                "/api/equipos/parte-equipos-maquinaria",
                "/parte-padre-actualizar-numero?qrString=" + this.QrCode,
                List,
                "bearer",
                MainViewModel.GetInstance().Token.Access_Token
                );
            //MainViewModel.GetInstance().EquipmentMachPartFoldingVariableViewModel = new EquipmentMachPartFoldingVariableViewModel(this.qrCode, this.SelectedUser, this.SelectedOperator, this.SelectedSewerGroup, this.PartNumber);
            //await App.Navigator.PushAsync(new EquipmentMachPartFoldingVariablePage());
        }

        #endregion

        //--
 
        //--

        //--
        private readonly ApiService apiService;
        public EquipmentMachPartFoldingVariableViewModel(string qrcode, string selectedUser, Guid selectedOperator, Guid selectedSewerGroup, string partNumber)
        {
            this.apiService = new ApiService();

            this.QrCode = qrcode;
            LoadMachines();
            LoadPhases();

            this.SelectedDate = DateTime.UtcNow;

            this.SelectedUserId = selectedUser;
            this.SelectedOperatorId = selectedOperator;
            this.SelectedSewerGroupId = selectedSewerGroup;
            this.PartNumber = partNumber;


        }
    }
}
