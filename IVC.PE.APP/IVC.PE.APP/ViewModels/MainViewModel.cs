using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Helpers;
using IVC.PE.APP.Common.Models;
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
using IVC.PE.APP.Views.General;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels
{
    public class MainViewModel
    {
        #region LogOutCommand
        public ICommand LogOutCommand => new RelayCommand(this.LogOut);
        private void LogOut()
        {
            Settings.IsRemember = false;
            Settings.Token = string.Empty;
            Settings.UserEmail = string.Empty;
            Settings.UserPassword = string.Empty;
            MainViewModel.GetInstance().LoginViewModel = new LoginViewModel();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        #endregion

        private static MainViewModel instance;
        public ObservableCollection<MenuGroupItemViewModel> Menus { get; set; }

        public Token Token { get; set; }

        #region General
        public LoginViewModel LoginViewModel { get; set; }
        public DashboardViewModel DashboardViewModel { get; set; }
        #endregion

        #region Production
        public AggregateViewModel AggregateViewModel { get; set; }
        #endregion

        #region Warehouse-Logistics
        public StockViewModel StockViewModel { get; set; }

        public TechoListViewModel TechoListViewModel { get; set; }

        public TechoListDetailViewModel TechoListDetailViewModel { get; set; }

        //--

        public VerificationCheckViewModel VerificationCheckViewModel { get; set; }

        public VerificationOptionSelectViewModel VerificationOptionSelectViewModel { get; set; }

        public VerificationQrCameraViewModel VerificationQrCameraViewModel { get; set; }

        public VerificationSupplyCheckViewModel VerificationSupplyCheckViewModel { get; set; }
                           
        public VerificationSupplyOptionSelectViewModel VerificationSupplyOptionSelectViewModel { get; set; }
                          
        public VerificationSupplyQrCameraViewModel VerificationSupplyQrCameraViewModel { get; set; }


        public FieldRequestCreateViewModel FieldRequestCreateViewModel { get; set; }
        public FieldRequestDetailCreateViewModel FieldRequestDetailCreateViewModel { get; set; }
        public FieldRequestEditViewModel FieldRequestEditViewModel { get; set; }
        public FieldRequestDetailEditViewModel FieldRequestDetailEditViewModel { get; set; }

        public FieldRequestCreateFormulaViewModel FieldRequestCreateFormulaViewModel { get; set; }

        public FieldRequestListViewModel FieldRequestListViewModel { get; set; }

        public FieldRequestListDetailViewModel FieldRequestListDetailViewModel { get; set; }

        public FieldRequestDetailAndFatherCreateViewModel FieldRequestDetailAndFatherCreateViewModel { get; set; }
        public FieldRequestDetailListDetailViewModel FieldRequestDetailListDetailViewModel { get; set; }

        public FieldRequestOptionSelectorViewModel FieldRequestOptionSelectorViewModel { get; set; }

        #endregion

        #region Technical Office
        public FootageViewModel FootageViewModel { get; set; }
        public FootageSewerBoxViewModel FootageSewerBoxViewModel { get; set; }

        public BluePrintViewModel BluePrintViewModel { get; set; }
        public BluePrintListViewModel BluePrintListViewModel { get; set; }
        //public ToScanBluePrintViewModel ToScanBluePrintViewModel { get; set; }
        public BluePrintPdfViewModel BluePrintPdfViewModel { get; set; }
        public BluePrintCheckViewModel BluePrintCheckViewModel { get; set; }
        public BluePrintOptionSelectViewModel BluePrintOptionSelectViewModel { get; set; }
        public BluePrintQrCameraViewModel BluePrintQrCameraViewModel { get; set; }
        public LetterPdfViewModel LetterPdfViewModel { get; set; }

        public BluePrintListDetailViewModel BluePrintListDetailViewModel { get; set; }

        //-----

        public BimListDetailViewModel BimListDetailViewModel { get; set; }

        public BimListViewModel BimListViewModel { get; set; }

        public BimPdfViewModel BimPdfViewModel { get; set; }

        //-----

        public MixDesignListDetailViewModel MixDesignListDetailViewModel { get; set; }

        public MixDesignListViewModel MixDesignListViewModel { get; set; }

        public MixDesignPdfViewModel MixDesignPdfViewModel { get; set; }

        //--

        public ProviderCatalogListDetailViewModel ProviderCatalogListDetailViewModel { get; set; }

        public ProviderCatalogListViewModel ProviderCatalogListViewModel { get; set; }

        public ProviderCatalogPdfViewModel ProviderCatalogPdfViewModel { get; set; }

        //--

        public TechnicalSpecListDetailViewModel TechnicalSpecListDetailViewModel { get; set; }

        public TechnicalSpecListViewModel TechnicalSpecListViewModel { get; set; }

        public TechnicalSpecPdfViewModel TechnicalSpecPdfViewModel { get; set; }

        //--

        public ProcedureListDetailViewModel ProcedureListDetailViewModel { get; set; }

        public ProcedureListViewModel ProcedureListViewModel { get; set; }

        public ProcedurePdfViewModel ProcedurePdfViewModel { get; set; }

        //--

        public ErpManualListDetailViewModel ErpManualListDetailViewModel { get; set; }

        public ErpManualListViewModel ErpManualListViewModel { get; set; }

        public ErpManualPdfViewModel ErpManualPdfViewModel { get; set; }
        #endregion

        #region Equipment
        public EquipmentMachPartCheckViewModel EquipmentMachPartCheckViewModel { get; set; }

        public EquipmentMachPartQrCameraViewModel EquipmentMachPartQrCameraViewModel { get; set; }
        public EquipmentMachineryOperatorScanQrCameraViewModel EquipmentMachineryOperatorScanQrCameraViewModel { get; set; }

        public EquipmentMachPartSelectOptionViewModel EquipmentMachPartSelectOptionViewModel { get; set; }

        public EquipmentMachPartFoldingVariableViewModel EquipmentMachPartFoldingVariableViewModel { get; set; }

        public EquipmentMachPartFoldingViewModel EquipmentMachPartFoldingViewModel { get; set; }
        #endregion

        #region Logistic
        public PreRequestListViewModel PreRequestListViewModel { get; set; }
        public OptionSelectorViewModel OptionSelectorViewModel { get; set; }

        public PreRequestIssuedListDetailViewModel PreRequestIssuedListDetailViewModel { get; set; }
        public PreRequestIssuedDetailListDetailViewModel PreRequestIssuedDetailListDetailViewModel { get; set; }
        public PreRequestIssuedListViewModel PreRequestIssuedListViewModel { get; set; }

        public PreRequestListDetailViewModel PreRequestListDetailViewModel { get; set; }

        public PreRequestDetailListDetailViewModel PreRequestDetailListDetailViewModel { get; set; }

        public PreRequestCreateViewModel PreRequestCreateViewModel { get; set; }
        public PreRequestEditViewModel PreRequestEditViewModel { get; set; }

        public PreRequestDetailCreateViewModel PreRequestDetailCreateViewModel { get; set; }
        public PreRequestDetailEditViewModel PreRequestDetailEditViewModel { get; set; }
        #endregion

        #region Security
        public RacsViewModel RacsViewModel { get; set; }
        public RacsListViewModel RacsListViewModel { get; set; }
        public RacsAddViewModel RacsAddViewModel { get; set; }
        public RacsLiftViewModel RacsLiftViewModel { get; set; }
        public RacsPdfViewModel RacsPdfViewModel { get; set; }
        #endregion

        #region HrWorker
        public WorkerAttendanceSearchViewModel WorkerAttendanceSearchViewModel { get; set; }
        public WorkerAttendanceCheckViewModel WorkerAttendanceCheckViewModel { get; set; }
        public WorkerAttendanceQrCameraViewModel WorkerAttendanceQrCameraViewModel { get; set; }
        public WorkerDailyTaskSearchViewModel WorkerDailyTaskSearchViewModel { get; set; }
        public WorkerDailyTaskListViewModel WorkerDailyTaskListViewModel { get; set; }
        public WorkerDailyTaskEditViewModel WorkerDailyTaskEditViewModel { get; set; }
        #endregion

        public MainViewModel()
        {
            instance = this;
            this.LoadMenus();
        }

        public static MainViewModel GetInstance()
        {
            if (instance == null)
                return new MainViewModel();

            return instance;
        }

        private void LoadMenus()
        {
            //Icons from https://fontawesome.com/cheatsheet/free/solid
            this.Menus = new ObservableCollection<MenuGroupItemViewModel>();

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.PRODUCTION],
                new ObservableCollection<MenuItemViewModel>(GetProductionMenuItems())
                ));

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.LOGISTIC],
                new ObservableCollection<MenuItemViewModel>(GetLogisticMenuItems())
                ));

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.WAREHOUSE],
                new ObservableCollection<MenuItemViewModel>(GetWarehouseMenuItems())
                ));

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.TECHNICAL_OFFICE],
                new ObservableCollection<MenuItemViewModel>(GetTechnicalOfficeMenuItems())
                ));

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.TECHNICAL_AREA],
                new ObservableCollection<MenuItemViewModel>(GetTechnicalAreaMenuItems())
                ));

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.EQUIPMENTS],
                new ObservableCollection<MenuItemViewModel>(GetEquipmentMenuItems())
                ));

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.QUALITY],
                new ObservableCollection<MenuItemViewModel>(GetQualityMenuItems())
                ));

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.SECURITY],
                new ObservableCollection<MenuItemViewModel>(GetSecurityMenuItems())
                ));

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.ENVIRONMENT],
                new ObservableCollection<MenuItemViewModel>(GetEnvironmentMenuItems())
                ));

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.HRWORKER],
                new ObservableCollection<MenuItemViewModel>(GetHRWorkerMenuItems())
                ));

            this.Menus.Add(new MenuGroupItemViewModel(
                ConstantHelpers.Menus.Groups.MENU_HEADERS[ConstantHelpers.Menus.Groups.SIG],
                new ObservableCollection<MenuItemViewModel>(GetSIGMenuItems())
                ));
        }

        private IEnumerable<MenuItemViewModel> GetHRWorkerMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Asistencia",
                    PageName = "WorkerAttendancePage",
                    GroupKey = ConstantHelpers.Menus.Groups.HRWORKER
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Tareo Diario",
                    PageName = "WorkerDailyTaskPage",
                    GroupKey = ConstantHelpers.Menus.Groups.HRWORKER
                }
            };

            return menus;
        }

        private List<MenuItemViewModel> GetProductionMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "RDP",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.PRODUCTION
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "RSP",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.PRODUCTION
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Análisis de Consumo",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.PRODUCTION
                },
                new MenuItemViewModel
                {
                    Icon = "\uf043",
                    Title = "Agregados y Similares",
                    PageName = "AggregatePage",
                    GroupKey = ConstantHelpers.Menus.Groups.PRODUCTION
                }
            };

            return menus;
        }

        private List<MenuItemViewModel> GetWarehouseMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Techos",
                    PageName = "TechoListPage",
                    GroupKey = ConstantHelpers.Menus.Groups.WAREHOUSE
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Pedidos de Campo",
                    PageName = "FieldRequestOptionSelectorPage",
                    GroupKey = ConstantHelpers.Menus.Groups.WAREHOUSE
                },
                //new MenuItemViewModel
                //{
                //    Icon = "\uf1c4",
                //    Title = "Pedidos por Atender",
                //    PageName = "ToAttendOrderPage",
                //    GroupKey = ConstantHelpers.Menus.Groups.WAREHOUSE
                //},
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Stocks",
                    PageName = "StockPage",
                    GroupKey = ConstantHelpers.Menus.Groups.WAREHOUSE
                },
                
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Entregas",
                    PageName = "DeliveryPage",
                    GroupKey = ConstantHelpers.Menus.Groups.WAREHOUSE
                },

                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Verificación de Equipos",
                    PageName = "VerificationOptionSelectPage",
                    GroupKey = ConstantHelpers.Menus.Groups.WAREHOUSE
                },

                                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Verificación de Insumos",
                    PageName = "VerificationSupplyOptionSelectPage",
                    GroupKey = ConstantHelpers.Menus.Groups.WAREHOUSE
                }
            };

            return menus;
        }

        private List<MenuItemViewModel> GetTechnicalOfficeMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "For-47",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_OFFICE
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "For-53",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_OFFICE
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "For-02",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_OFFICE
                },
                new MenuItemViewModel
                {
                    Icon = "\uf546",
                    Title = "Metrados",
                    PageName = "FootagePage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_OFFICE
                },

                //                new MenuItemViewModel
                //{
                //    Icon = "\uf1c4",
                //    Title = "Planos",
                //    PageName = "BluePrintListPage",
                //    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_OFFICE
                //}
            };

            return menus;
        }

        //
        private List<MenuItemViewModel> GetLogisticMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {


                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Generación Pre-Requerimientos",
                    PageName = "PreRequestCreatePage",
                    GroupKey = ConstantHelpers.Menus.Groups.LOGISTIC
                },

               new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Listado Pre-Requerimientos",
                    PageName = "PreRequestIssuedListPage",
                    GroupKey = ConstantHelpers.Menus.Groups.LOGISTIC
                },

                //                new MenuItemViewModel
                //{
                //    Icon = "\uf1c4",
                //    Title = "Planos",
                //    PageName = "BluePrintListPage",
                //    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_OFFICE
                //}
            };

            return menus;
        }
        //

        private List<MenuItemViewModel> GetTechnicalAreaMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Repositorio de Planos",
                    PageName = "BluePrintListPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_AREA
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "QR Planos",
                    PageName = "BluePrintOptionSelectPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_AREA
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Repositorio de BIMs",
                    PageName = "BimListPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_AREA
                },

                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Repositorio de Especificaciones Técnicas",
                    PageName = "TechnicalSpecListPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_AREA
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Repositorio de Diseños de Mezcla",
                    PageName = "MixDesignListPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_AREA
                },

                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Repositorio de Catálogo de Proveedores",
                    PageName = "ProviderCatalogListPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_AREA
                }
                ,

                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Repositorio de Manuales ERP",
                    PageName = "ErpManualListPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_AREA
                }
            };

            return menus;
        }


        private List<MenuItemViewModel> GetEquipmentMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {

                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Equipos - parte diario Maquinarias",
                    PageName = "EquipmentMachPartSelectOptionPage",
                    GroupKey = ConstantHelpers.Menus.Groups.EQUIPMENTS
                }
            };

            return menus;
        }

        private List<MenuItemViewModel> GetQualityMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Densidad de Campo",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.QUALITY
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Pruebas Hidraúlicas",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.QUALITY
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Concreto",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.QUALITY
                },
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Indicadores",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.QUALITY
                }
            };

            return menus;
        }

        private List<MenuItemViewModel> GetSecurityMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "\uf46c",
                    Title = "RACS",
                    PageName = "RacsReportsPage",
                    GroupKey = ConstantHelpers.Menus.Groups.SECURITY
                },
                new MenuItemViewModel
                {
                    Icon = "\uf46c",
                    Title = "ATS",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.SECURITY
                },
                new MenuItemViewModel
                {
                    Icon = "\uf46c",
                    Title = "Indicadores",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.SECURITY
                }
            };

            return menus;
        }

        private List<MenuItemViewModel> GetEnvironmentMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "\uf46c",
                    Title = "Indicadores",
                    PageName = "NoPage",
                    GroupKey = ConstantHelpers.Menus.Groups.ENVIRONMENT
                }
            };

            return menus;
        }

        private List<MenuItemViewModel> GetSIGMenuItems()
        {
            var menus = new List<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "\uf1c4",
                    Title = "Documentación",
                    PageName = "ProcedureListPage",
                    GroupKey = ConstantHelpers.Menus.Groups.TECHNICAL_AREA
                }
            };

            return menus;
        }
    }
}
