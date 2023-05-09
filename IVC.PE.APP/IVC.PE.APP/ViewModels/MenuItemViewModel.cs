using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.ViewModels.EquipmentMachinery;
using IVC.PE.APP.ViewModels.General;
using IVC.PE.APP.ViewModels.HrWorker.Attendance;
using IVC.PE.APP.ViewModels.HrWorker.DailyTask;
using IVC.PE.APP.ViewModels.Logistic.PreRequests;
using IVC.PE.APP.ViewModels.Production.Aggregate;
using IVC.PE.APP.ViewModels.Security.Racs;
using IVC.PE.APP.ViewModels.TechnicalOffice.Bims;
using IVC.PE.APP.ViewModels.TechnicalOffice.BluePrints;
using IVC.PE.APP.ViewModels.TechnicalOffice.ErpManuals;
using IVC.PE.APP.ViewModels.TechnicalOffice.Footages;
using IVC.PE.APP.ViewModels.TechnicalOffice.MixDesigns;
using IVC.PE.APP.ViewModels.TechnicalOffice.Procedure;
using IVC.PE.APP.ViewModels.TechnicalOffice.ProviderCatalogs;
using IVC.PE.APP.ViewModels.TechnicalOffice.TechnicalSpecs;
using IVC.PE.APP.ViewModels.Warehouse.FieldRequests;
using IVC.PE.APP.ViewModels.Warehouse.Stocks;
using IVC.PE.APP.ViewModels.Warehouse.Techos;
using IVC.PE.APP.ViewModels.Warehouse.Verifications;
using IVC.PE.APP.Views.EquipmentMachinery;
using IVC.PE.APP.Views.General;
using IVC.PE.APP.Views.HrWorker.Attendance;
using IVC.PE.APP.Views.HrWorker.DailyTask;
using IVC.PE.APP.Views.Logistic.PreRequests;
using IVC.PE.APP.Views.Production.Aggregate;
using IVC.PE.APP.Views.Security.Racs;
using IVC.PE.APP.Views.TechincalOffice.ProviderCatalogs;
using IVC.PE.APP.Views.TechnicalOffice.Bims;
using IVC.PE.APP.Views.TechnicalOffice.BluePrints;
using IVC.PE.APP.Views.TechnicalOffice.ErpManuals;
using IVC.PE.APP.Views.TechnicalOffice.Footages;
using IVC.PE.APP.Views.TechnicalOffice.MixDesigns;
using IVC.PE.APP.Views.TechnicalOffice.Procedures;
using IVC.PE.APP.Views.TechnicalOffice.TechnicalSpecs;
using IVC.PE.APP.Views.Warehouse.FieldRequests;
using IVC.PE.APP.Views.Warehouse.Stocks;
using IVC.PE.APP.Views.Warehouse.Techos;
using IVC.PE.APP.Views.Warehouse.Verification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels
{
    public class MenuItemViewModel : Common.Models.Menu
    {
        public ICommand SelectMenuCommand => new RelayCommand(this.SelectMenu);

        private void SelectMenu()
        {
            App.Master.IsPresented = false;

            if (this.PageName == "NoPage")
                return;

            switch (this.GroupKey)
            {
                case ConstantHelpers.Menus.Groups.GENERAL:
                    GeneralNavigation(this.PageName);
                    break;
                case ConstantHelpers.Menus.Groups.PRODUCTION:
                    ProductionNavigation(this.PageName);
                    break;

                case ConstantHelpers.Menus.Groups.LOGISTIC:
                    LogisticNavigation(this.PageName);
                    break;

                case ConstantHelpers.Menus.Groups.WAREHOUSE:
                    WarehouseNavigation(this.PageName);
                    break;
                case ConstantHelpers.Menus.Groups.TECHNICAL_OFFICE:
                    TechnicalOfficeNavigation(this.PageName);
                    break;
                case ConstantHelpers.Menus.Groups.EQUIPMENTS:
                    EquipmentNavigation(this.PageName);
                    break;
                case ConstantHelpers.Menus.Groups.TECHNICAL_AREA:
                    TechnicalAreaNavigation(this.PageName);
                    break;
                case ConstantHelpers.Menus.Groups.QUALITY:
                    QualityNavigation(this.PageName);
                    break;
                case ConstantHelpers.Menus.Groups.SECURITY:
                    SecurityNavigation(this.PageName);
                    break;
                case ConstantHelpers.Menus.Groups.ENVIRONMENT:
                    EnvironmentNavigation(this.PageName);
                    break;
                case ConstantHelpers.Menus.Groups.HRWORKER:
                    HRWorkerNavigation(this.PageName);
                    break;
            }
        }

        private void EnvironmentNavigation(string pageName)
        {
            switch (this.PageName)
            {
                default:
                    Logout();
                    break;
            }
        }

        private async void SecurityNavigation(string pageName)
        {
            switch (this.PageName)
            {
                case "RacsReportsPage":
                    MainViewModel.GetInstance().RacsListViewModel = new RacsListViewModel();
                    await App.Navigator.PushAsync(new RacsListPage());
                    break;
                default:
                    Logout();
                    break;
            }
        }

        private async void QualityNavigation(string pageName)
        {
            switch (this.PageName)
            {   
                default:
                    Logout();
                    break;
            }
        }

        private async void TechnicalOfficeNavigation(string pageName)
        {
            switch (this.PageName)
            {
                case "FootagePage":
                    MainViewModel.GetInstance().FootageViewModel = new FootageViewModel();
                    await App.Navigator.PushAsync(new FootagePage());
                    break;

                default:
                    Logout();
                    break;
            }
        }

        private async void EquipmentNavigation(string pageName)
        {
            switch (this.PageName)
            {
                case "EquipmentMachPartSelectOptionPage":
                    MainViewModel.GetInstance().EquipmentMachPartSelectOptionViewModel = new EquipmentMachPartSelectOptionViewModel();
                    await App.Navigator.PushAsync(new EquipmentMachPartSelectOptionPage());
                    break;

                default:
                    Logout();
                    break;
            }
        }

        //
        private async void LogisticNavigation(string pageName)
        {
            switch (this.PageName)
            {

                case "PreRequestIssuedListPage":
                    MainViewModel.GetInstance().PreRequestIssuedListViewModel = new PreRequestIssuedListViewModel();
                    await App.Navigator.PushAsync(new PreRequestIssuedListPage());
                    break;

                case "PreRequestCreatePage":
                    MainViewModel.GetInstance().OptionSelectorViewModel = new OptionSelectorViewModel();
                    await App.Navigator.PushAsync(new OptionSelectorPage());
                    break;
                default:
                    Logout();
                    break;
            }
        }
        //

        private async void TechnicalAreaNavigation(string pageName)
        {
            switch (this.PageName)
            {

                case "BluePrintListPage":
                    MainViewModel.GetInstance().BluePrintListViewModel = new BluePrintListViewModel();
                    await App.Navigator.PushAsync(new BluePrintListPage());
                    break;

                case "BluePrintOptionSelectPage":
                    MainViewModel.GetInstance().BluePrintOptionSelectViewModel = new BluePrintOptionSelectViewModel();
                    await App.Navigator.PushAsync(new BluePrintOptionSelectPage());
                    break;

                case "BimListPage":
                    MainViewModel.GetInstance().BimListViewModel = new BimListViewModel();
                    await App.Navigator.PushAsync(new BimListPage());
                    break;

                case "MixDesignListPage":
                    MainViewModel.GetInstance().MixDesignListViewModel = new MixDesignListViewModel();
                    await App.Navigator.PushAsync(new MixDesignListPage());
                    break;

                case "ProviderCatalogListPage":
                    MainViewModel.GetInstance().ProviderCatalogListViewModel = new ProviderCatalogListViewModel();
                    await App.Navigator.PushAsync(new ProviderCatalogListPage());
                    break;

                case "ProcedureListPage":
                    MainViewModel.GetInstance().ProcedureListViewModel = new ProcedureListViewModel();
                    await App.Navigator.PushAsync(new ProcedureListPage());
                    break;

                case "TechnicalSpecListPage":
                    MainViewModel.GetInstance().TechnicalSpecListViewModel = new TechnicalSpecListViewModel();
                    await App.Navigator.PushAsync(new TechnicalSpecListPage());
                    break;

                case "ErpManualListPage":
                    MainViewModel.GetInstance().ErpManualListViewModel = new ErpManualListViewModel();
                    await App.Navigator.PushAsync(new ErpManualListPage());
                    break;
                default:
                    Logout();
                    break;
            }
        }
        private async void WarehouseNavigation(string pageName)
        {
            switch (this.PageName)
            {
                case "TechoListPage":
                    MainViewModel.GetInstance().TechoListViewModel = new TechoListViewModel();
                    await App.Navigator.PushAsync(new TechoListPage());
                    break;
                case "FieldRequestOptionSelectorPage":
                    MainViewModel.GetInstance().FieldRequestOptionSelectorViewModel = new FieldRequestOptionSelectorViewModel();
                    await App.Navigator.PushAsync(new FieldRequestOptionSelectorPage());
                    break;
                case "ToAttendOrderPage":
                    break;
                case "StockPage":
                    MainViewModel.GetInstance().StockViewModel = new StockViewModel();
                    await App.Navigator.PushAsync(new StockPage());
                    break;
                case "DeliveryPage":
                    break;
                case "CheckPage":
                    break;
                case "VerificationOptionSelectPage":
                    MainViewModel.GetInstance().VerificationOptionSelectViewModel = new VerificationOptionSelectViewModel();
                    await App.Navigator.PushAsync(new VerificationOptionSelectPage());
                    break;
                case "VerificationSupplyOptionSelectPage":
                    MainViewModel.GetInstance().VerificationSupplyOptionSelectViewModel = new VerificationSupplyOptionSelectViewModel();
                    await App.Navigator.PushAsync(new VerificationSupplyOptionSelectPage());
                    break;
                default:
                    Logout();
                    break;
            }
        }

        private async void ProductionNavigation(string pageName)
        {
            switch (this.PageName)
            {
                case "AggregatePage":
                    MainViewModel.GetInstance().AggregateViewModel = new AggregateViewModel();
                    await App.Navigator.PushAsync(new AggregatePage());
                    break;
                default:
                    Logout();
                    break;
            }
        }

        private async void GeneralNavigation(string pageName)
        {
            switch (this.PageName)
            {
                case "DashboardPage":
                    MainViewModel.GetInstance().DashboardViewModel = new DashboardViewModel();
                    await App.Navigator.PushAsync(new DashboardPage());
                    break;
                default:
                    Logout();
                    break;
            }
        }

        private async void HRWorkerNavigation(string pageName)
        {
            switch (this.PageName)
            {
                case "WorkerAttendancePage":
                    MainViewModel.GetInstance().WorkerAttendanceSearchViewModel = new WorkerAttendanceSearchViewModel();
                    await App.Navigator.PushAsync(new WorkerAttendanceSearchPage());
                    break;
                case "WorkerDailyTaskPage":
                    MainViewModel.GetInstance().WorkerDailyTaskSearchViewModel = new WorkerDailyTaskSearchViewModel();
                    await App.Navigator.PushAsync(new WorkerDailyTaskSearchPage());
                    break;
                default:
                    Logout();
                    break;
            }
        }

        private void Logout()
        {
            Settings.IsRemember = false;
            Settings.Token = string.Empty;
            Settings.UserEmail = string.Empty;
            Settings.UserPassword = string.Empty;
            MainViewModel.GetInstance().LoginViewModel = new LoginViewModel();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

    public class MenuGroupItemViewModel : ObservableCollection<MenuItemViewModel>
    {
        public string Name { get; private set; }

        public MenuGroupItemViewModel(string name) : base()
        {
            this.Name = name;
        }

        public MenuGroupItemViewModel(string name, IEnumerable<MenuItemViewModel> source) : base(source)
        {
            this.Name = name;
        }
    }
}
