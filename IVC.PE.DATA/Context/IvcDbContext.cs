using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.ENTITIES.Models.Aggregation;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.ENTITIES.Models.LegalTechnicalLibrary;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.UspModels.Biddings;
using IVC.PE.ENTITIES.UspModels.HumanResources;
using IVC.PE.ENTITIES.Models.Finance;
using IVC.PE.ENTITIES.Models.Security;
using IVC.PE.ENTITIES.UspModels.Finances;
using IVC.PE.ENTITIES.UspModels.Quality;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.ENTITIES.Models.Production;
using IVC.PE.ENTITIES.UspModels.Security;
using IVC.PE.ENTITIES.UspModels.DocumentaryControl;
using IVC.PE.ENTITIES.UspModels.Dashboard;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.ENTITIES.Models.Temporal;
using IVC.PE.ENTITIES.UspModels.Production;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.ENTITIES.Models.IntegratedManagementSystem;
using IVC.PE.ENTITIES.UspModels.General;
using IVC.PE.ENTITIES.UspModels.WareHouse;
using IVC.PE.ENTITIES.UspModels.Security.Training;
using IVC.PE.ENTITIES.Models.Accounting;

namespace IVC.PE.DATA.Context
{
    public class IvcDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
                            ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>,
                            IdentityUserToken<string>>
    {
        #region General
        public DbSet<ApplicationUserInterestGroup> UserInterestGroups { get; set; }
        public DbSet<ApplicationUserProject> UserProjects { get; set; }
        public DbSet<CertificateIssuer> CertificateIssuers { get; set; }
        public DbSet<InterestGroup> InterestGroups { get; set; }
        public DbSet<InterestGroupEmail> InterestGroupEmails { get; set; }
        public DbSet<Foreman> Foremen { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectCollaborator> ProjectCollaborators { get; set; }
        public DbSet<ProjectPhase> ProjectPhases { get; set; }
        public DbSet<ProjectHabilitation> ProjectHabilitations { get; set; }
        public DbSet<QualificationZone> QualificationZones { get; set; }
        public DbSet<SystemPhase> SystemPhases { get; set; }
        public DbSet<WorkFront> WorkFronts { get; set; }
        public DbSet<WorkFrontProjectPhase> WorkFrontProjectPhases { get; set; }
        public DbSet<WorkFrontSewerGroup> WorkFrontSewerGroups { get; set; }
        public DbSet<WorkFrontHead> WorkFrontHeads { get; set; }
        public DbSet<WorkPosition> WorkPositions { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<ENTITIES.Models.General.WorkComponent> WorkComponents { get; set; }
        public DbSet<WorkStructure> WorkStructures { get; set; }
        public DbSet<ProjectFormula> ProjectFormulas { get; set; }
        public DbSet<ProjectFormulaActivity> ProjectFormulaActivities { get; set; }
        public DbSet<ProjectFormulaSewerGroup> ProjectFormulaSewerGroups { get; set; }
        public DbSet<WorkArea> WorkAreas { get; set; }
        public DbSet<WorkAreaItem> WorkAreaItems { get; set; }
        public DbSet<WorkRole> WorkRoles { get; set; }
        public DbSet<WorkRoleItem> WorkRoleItems { get; set; }
        #endregion

        #region Aggregation
        public DbSet<AggregationEntry> AggregationEntries { get; set; }
        public DbSet<AggregationPrice> AggregationPrices { get; set; }
        public DbSet<AggregationProvider> AggregationProviders { get; set; }
        public DbSet<AggregationProviderType> AggregationProviderTypes { get; set; }
        public DbSet<AggregationStock> AggregationStocks { get; set; }
        public DbSet<AggregationStockType> AggregationStockTypes { get; set; }
        public DbSet<AggregationRequest> AggregationRequests { get; set; }
        public DbSet<Quarry> Quarries { get; set; }
        #endregion

        #region Bidding
        public DbSet<LegalDocumentationLoad> LegalDocumentationLoads { get; set; }
        public DbSet<LegalDocumentation> LegalDocumentations { get; set; }
        public DbSet<LegalDocumentationRenovation> LegalDocumentationRenovations { get; set; }
        public DbSet<LegalDocumentationType> LegalDocumentationTypes { get; set; }
        public DbSet<LegalDocumentationRenovationApplicationUser> LegalDocumentationRenovationApplicationUsers { get; set; }
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<BusinessResponsible> BusinessResponsibles { get; set; }

        public DbSet<BiddingBusiness> BiddingBusinesses { get; set; }

        public DbSet<BiddingWork> BiddingWorks { get; set; }

        public DbSet<BiddingWorkComponent> BiddingWorkComponents { get; set; }

        public DbSet<BiddingWorkComponentDetail> BiddingWorkComponentDetails { get; set; }

        public DbSet<College> Colleges { get; set; }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<BiddingCurrencyType> BiddingCurrencyTypes { get; set; }

        public DbSet<BiddingWorkType> BiddingWorkTypes { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<ProfessionalExperienceFolding> ProfessionalExperienceFoldings { get; set; }






        public DbSet<SkillRenovation> SkillRenovations { get; set; }
        public DbSet<SkillRenovationApplicationUser> SkillRenovationApplicationUsers { get; set; }
        public DbSet<Skill> Skills { get; set; }
        #endregion

        #region DocumentaryControl

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionRenovation> PermissionRenovations { get; set; }
        public DbSet<PermissionRenovationApplicationUser> PermissionRenovationApplicationUsers { get; set; }
        public DbSet<IssuerTarget> IssuerTargets { get; set; }
        public DbSet<Letter> Letters { get; set; }

        public DbSet<LetterResponsible> LetterResponsibles { get; set; }
        //public DbSet<LetterAuthorization> LetterAuthorizations { get; set; }
        public DbSet<LetterLetter> LetterLetters { get; set; }
        public DbSet<LetterInterestGroup> LetterInterestGroups { get; set; }
        public DbSet<LetterIssuerTarget> LetterIssuerTargets { get; set; }
        public DbSet<LetterStatus> LetterStatus { get; set; }
        public DbSet<LetterDocumentCharacteristic> LetterDocumentCharacteristics { get; set; }
        public DbSet<Workbook> Workbooks { get; set; }
        public DbSet<WorkbookType> WorkbookTypes { get; set; }
        public DbSet<WorkbookSeat> WorkbookSeats { get; set; }
        public DbSet<AuthorizationType> AuthorizationTypes { get; set; }
        public DbSet<AuthorizingEntity> AuthorizingEntities { get; set; }
        public DbSet<PermissionProjectResponsible> PermissionProjectResponsibles { get; set; }
        public DbSet<RenovationType> RenovationTypes { get; set; }
        #endregion

        #region Equipment Machinery

        public DbSet<InsuranceEntity> InsuranceEntity { get; set; }
        public DbSet<Equipment> Equipment { get; set; }

        public DbSet<EquipmentMachineryCalendarWeek> EquipmentMachineryCalendarWeeks { get; set; }


        public DbSet<MachineryPhase> MachineryPhases { get; set; }

        public DbSet<TransportPhase> TransportPhases { get; set; }

        public DbSet<EquipmentMach> EquipmentMachs { get; set; }

        public DbSet<EquipmentMachSOATFolding> EquipmentMachSOATFoldings { get; set; }
        public DbSet<EquipmentMachTechnicalRevisionFolding> EquipmentMachTechnicalRevisions { get; set; }

        public DbSet<EquipmentMachInsuranceFolding> EquipmentMachInsuranceFoldings { get; set; }

        public DbSet<EquipmentMachInsuranceFoldingApplicationUser> EquipmentMachInsuranceFoldingApplicationUsers { get; set; }
        public DbSet<EquipmentMachSOATFoldingApplicationUser> EquipmentMachSOATFoldingApplicationUsers { get; set; }
        public DbSet<EquipmentMachTechnicalRevisionFoldingApplicationUser> EquipmentMachTechnicalRevisionFoldingApplications { get; set; }


        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<EquipmentMachineryResponsible> EquipmentMachineryResponsibles { get; set; }
        public DbSet<EquipmentMachineryType> EquipmentMachineryTypes { get; set; }
        public DbSet<EquipmentMachineryTypeType> EquipmentMachineryTypeTypes { get; set; }
        public DbSet<EquipmentMachineryTypeSoft> EquipmentMachineryTypeSofts { get; set; }
        public DbSet<EquipmentMachineryTypeTransport> EquipmentMachineryTypeTransports { get; set; }
        public DbSet<EquipmentMachineryTypeTypeActivity> EquipmentMachineryTypeTypeActivities { get; set; }
        public DbSet<EquipmentMachineryTypeSoftActivity> EquipmentMachineryTypeSoftActivites { get; set; }
        public DbSet<EquipmentMachineryTypeTransportActivity> EquipmentMachineryTypeTransportActivities { get; set; }
        public DbSet<EquipmentMachineryOperator> EquipmentMachineryOperators { get; set; }


        public DbSet<FuelProvider> FuelProviders { get; set; }
        public DbSet<FuelProviderFolding> FuelProviderFoldings { get; set; }
        public DbSet<FuelProviderPriceFolding> FuelProviderPriceFoldings { get; set; }

        public DbSet<EquipmentProvider> EquipmentProviders { get; set; }
        public DbSet<EquipmentProviderFolding> EquipmentProviderFoldings { get; set; }
        public DbSet<EquipmentMachineryCharacteristic> EquipmentMachineryCharacteristics { get; set; }
        public DbSet<EquipmentMachinerySoft> EquipmentMachinerySofts { get; set; }
        public DbSet<EquipmentMachinerySoftFolding> EquipmentMachinerySoftFoldings { get; set; }
        public DbSet<EquipmentMachinerySoftInsuranceFolding> EquipmentMachinerySoftInsuranceFoldings { get; set; }
        public DbSet<EquipmentMachinerySoftInsuranceFoldingApplicationUser> EquipmentMachinerySoftInsuranceFoldingApplicationUsers { get; set; }
        public DbSet<EquipmentMachineryTransport> EquipmentMachineryTransports { get; set; }
        public DbSet<EquipmentMachineryTransportInsuranceFolding> EquipmentMachineryTransportInsuranceFoldings { get; set; }
        public DbSet<EquipmentMachineryTransportSOATFolding> EquipmentMachineryTransportSOATFoldings { get; set; }
        public DbSet<EquipmentMachineryTransportTechnicalRevisionFolding> EquipmentMachineryTransportTechnicalRevisions { get; set; }
        public DbSet<EquipmentMachinerySoftApplicationUser> EquipmentMachinerySoftApplicationUsers { get; set; }
        public DbSet<EquipmentMachineryTransportInsuranceFoldingApplicationUser> EquipmentMachineryTransportInsuranceFoldingApplicationUsers { get; set; }
        public DbSet<EquipmentMachineryTransportSOATFoldingApplicationUser> EquipmentMachineryTransportSOATFoldingApplicationUsers { get; set; }
        public DbSet<EquipmentMachineryTransportTechnicalRevisionFoldingApplicationUser> EquipmentMachineryTransportTechnicalRevisionFoldingApplications { get; set; }
        public DbSet<EquipmentMachinerySoftPart> EquipmentMachinerySoftParts { get; set; }
        public DbSet<EquipmentMachinerySoftPartPlus> EquipmentMachinerySoftPartPlusUltra { get; set; }
        public DbSet<EquipmentMachinerySoftPartFolding> EquipmentMachinerySoftPartFoldings { get; set; }

        public DbSet<EquipmentMachineryTransportPart> EquipmentMachineryTransportParts { get; set; }
        public DbSet<EquipmentMachineryTransportPartPlus> EquipmentMachineryTransportPartPlusUltra { get; set; }
        public DbSet<EquipmentMachineryTransportPartFolding> EquipmentMachineryTransportPartFoldings { get; set; }

        public DbSet<EquipmentMachineryFuelTransportPart> EquipmentMachineryFuelTransportParts { get; set; }

        public DbSet<EquipmentMachineryFuelTransportPartFolding> EquipmentMachineryFuelTransportPartFoldings { get; set; }


        public DbSet<EquipmentMachineryFuelMachPart> EquipmentMachineryFuelMachParts { get; set; }

        public DbSet<EquipmentMachineryFuelMachPartFolding> EquipmentMachineryFuelMachPartFoldings { get; set; }



        public DbSet<EquipmentMachPart> EquipmentMachParts { get; set; }
        public DbSet<EquipmentMachPartFolding> EquipmentMachPartFoldings { get; set; }

        #endregion

        #region Finances
        public DbSet<BondType> BondTypes { get; set; }
        public DbSet<BondRenovation> BondRenovations { get; set; }
        public DbSet<BondGuarantor> BondGuarantors { get; set; }
        public DbSet<BondLoad> BondLoads { get; set; }
        public DbSet<BondAdd> BondAdds { get; set; }
        public DbSet<BondFile> BondFiles { get; set; }
        public DbSet<BondRenovationApplicationUser> BondRenovationApplicationUsers { get; set; }
        public DbSet<BondAddProjectResponsible> BondAddProjectResponsibles { get; set; }

        #endregion

        #region Human Resources
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeWorkPeriod> EmployeeWorkPeriods { get; set; }
        public DbSet<PensionFundAdministrator> PensionFundAdministrators { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkerWorkPeriod> WorkerWorkPeriods { get; set; }
        public DbSet<WorkerPosition> WorkerPositions { get; set; }
        public DbSet<ProjectCalendar> ProjectCalendars { get; set; }
        public DbSet<ProjectCalendarWeek> ProjectCalendarWeeks { get; set; }
        public DbSet<ProjectCalendarWeekFile> ProjectCalendarWeekFiles { get; set; }
        public DbSet<ProjectCalendarMonth> ProjectCalendarMonths { get; set; }
        public DbSet<WorkerDailyTask> WorkerDailyTasks { get; set; }
        public DbSet<PayrollVariable> PayrollVariables { get; set; }
        public DbSet<PayrollConcept> PayrollConcepts { get; set; }
        public DbSet<PayrollConceptFormula> PayrollConceptFormulas { get; set; }
        public DbSet<PayrollPensionFundAdministratorRate> PayrollPensionFundAdministratorRates { get; set; }
        public DbSet<PayrollWorkerCategoryWage> PayrollWorkerCategoryWages { get; set; }
        public DbSet<PayrollMovementHeader> PayrollMovementHeaders { get; set; }
        public DbSet<PayrollMovementDetail> PayrollMovementDetails { get; set; }
        public DbSet<PayrollWorkerVariable> PayrollWorkerVariables { get; set; }
        public DbSet<PayrollWeekSummary> PayrollWeekSummaries { get; set; }
        public DbSet<ProjectPayrollResponsible> ProjectPayrollResponsibles { get; set; }
        public DbSet<PayrollAuthorizationRequest> PayrollAuthorizationRequests { get; set; }
        public DbSet<PayrollParameter> PayrollParameters { get; set; }
        public DbSet<WorkerCategory> WorkerCategories { get; set; }
        public DbSet<WorkerCovidCheck> WorkerCovidChecks { get; set; }
        public DbSet<WorkerMedicalRest> WorkerMedicalRests { get; set; }
        public DbSet<WorkerInvoiceSend> WorkerInvoiceSends { get; set; }
        public DbSet<WorkerFixedConcept> WorkerFixedConcepts { get; set; }
        #endregion

        #region IntegratedManagementSystem

        public DbSet<NewSIGProcess> NewSIGProcesses { get; set; }
        public DbSet<SewerManifoldFor24FirstPart> SewerManifoldFor24FirstParts { get; set; }
        public DbSet<For24FirstPartGallery> For24FirstPartGalleries { get; set; }
        public DbSet<SewerManifoldFor24SecondPart> SewerManifoldFor24SecondParts { get; set; }
        public DbSet<For24SecondPartGallery> For24SecondPartGalleries { get; set; }
        public DbSet<For24SecondPartAction> For24SecondPartActions { get; set; }
        public DbSet<For24SecondPartEquipment> For24SecondPartEquipments { get; set; }
        public DbSet<SewerManifoldFor24ThirdPart> SewerManifoldFor24ThirdParts { get; set; }
        public DbSet<SewerManifoldFor24> SewerManifoldFor24s { get; set; }
        #endregion

        #region Security
        public DbSet<RacsSummary> RacsSummaries { get; set; }
        public DbSet<RacsReport> RacsReports { get; set; }
        public DbSet<BehaviourReport> BehaviourReports { get; set; }
        public DbSet<BehaviourReportDetail> BehaviourReportDetails { get; set; }
        public DbSet<BehaviourReportItem> BehaviourReportItems { get; set; }
        public DbSet<BehaviourReportCause> BehaviourReportCauses { get; set; }
        public DbSet<BehaviourReportSummary> BehaviourReportSummaries { get; set; }
        public DbSet<TrainingResultStatus> TrainingResultStatus { get; set; }
        public DbSet<TrainingCategory> TrainingCategories { get; set; }
        public DbSet<TrainingTopic> TrainingTopics { get; set; }
        public DbSet<TrainingTopicFile> TrainingTopicFiles { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<TrainingSessionWorkerEmployee> TrainingSessionWorkerEmployees { get; set; }
        #endregion

        #region Legal Technical Library
        public DbSet<TechnicalLibraryFile> TechnicalLibraryFiles { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<IsoStandard> IsoStandards { get; set; }
        #endregion

        #region Logistics
        public DbSet<LogisticResponsible> LogisticResponsibles { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessParticipationFolding> BusinessParticipationFoldings { get; set; }

        public DbSet<BusinessLegalAgentFolding> BusinessLegalAgentFoldings { get; set; }
        public DbSet<BusinessFile> BusinessFiles { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderFile> ProviderFiles { get; set; }
        public DbSet<ProviderSupplyFamily> ProviderSupplyFamilies { get; set; }
        public DbSet<ProviderSupplyGroup> ProviderSupplyGroups { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestSummary> RequestSummaries { get; set; }
        public DbSet<RequestItem> RequestItems { get; set; }
        //public DbSet<RequestUser> RequestUsers { get; set; }
        public DbSet<RequestFile> RequestFiles { get; set; }
        public DbSet<RequestAuthorization> RequestAuthorizations { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<SupplyFamily> SupplyFamilies { get; set; }
        public DbSet<SupplyGroup> SupplyGroups { get; set; }
        public DbSet<PreRequest> PreRequests { get; set; }
        //public DbSet<PreRequestUser> PreRequestUsers { get; set; }
        public DbSet<PreRequestItem> PreRequestItems { get; set; }
        public DbSet<PreRequestFile> PreRequestFiles { get; set; }
        public DbSet<RequestsInOrder> RequestsInOrders { get; set; }
        public DbSet<OrderAuthorization> OrderAuthorizations { get; set; }
        public DbSet<PreRequestAuthorization> PreRequestAuthorizations { get; set; }
        #endregion

        #region Quality
        public DbSet<ConcreteQualityCertificate> ConcreteQualityCertificates { get; set; }
        public DbSet<ConcreteQualityCertificateDetail> ConcreteQualityCertificateDetails { get; set; }
        public DbSet<CompactionDensityCertificate> CompactionDensityCertificates { get; set; }
        public DbSet<CompactionDensityCertificateDetail> CompactionDensityCertificateDetails { get; set; }
        public DbSet<DischargeManifold> DischargeManifolds { get; set; }
        public DbSet<FillingLaboratoryTest> FillingLaboratoryTests { get; set; }
        public DbSet<OriginTypeFillingLaboratory> OriginTypeFillingLaboratories { get; set; }
        public DbSet<EquipmentCertificate> EquipmentCertificates { get; set; }
        public DbSet<EquipmentCertificateRenewal> EquipmentCertificateRenewals { get; set; }
        public DbSet<EquipmentCertificateRenewalApplicationUser> EquipmentCertificateRenewalApplicationUsers { get; set; }
        public DbSet<EquipmentCertifyingEntity> EquipmentCertifyingEntities { get; set; }
        public DbSet<EquipmentCertificateOwner> EquipmentOwners { get; set; }
        public DbSet<EquipmentCertificateUserOperator> EquipmentCertificateUserOperators { get; set; }
        public DbSet<EquipmentCertificateResponsible> EquipmentResponsibles { get; set; }
        public DbSet<EquipmentCertificateType> EquipmentCertificateTypes { get; set; }
        public DbSet<SigProcess> SigProcesses { get; set; }
        public DbSet<InstructionalProcedure> InstructionalProcedures { get; set; }
        public DbSet<PatternCalibration> PatternCalibrations { get; set; }
        public DbSet<PatternCalibrationRenewal> PatternCalibrationRenewals { get; set; }
        public DbSet<PatternCalibrationRenewalApplicationUser> PatternCalibrationRenewalApplicationUsers { get; set; }

        public DbSet<QualityFront> QualityFronts { get; set; }
        public DbSet<SewerManifoldFor47> SewerManifoldFor47s { get; set; }
        public DbSet<SewerManifoldFor05> SewerManifoldFor05s { get; set; }
        public DbSet<FoldingFor05> FoldingFor05s { get; set; }
        public DbSet<SewerManifoldFor29> SewerManifoldFor29s { get; set; }
        public DbSet<SewerManifoldFor37A> SewerManifoldFor37As { get; set; }
        public DbSet<FoldingFor37A> FoldingFor37As { get; set; }

        #endregion

        #region Technical Office

        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessTypeDocument> ProcessTypeDocuments { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<ProcedureFile> ProcedureFiles { get; set; }
        public DbSet<ProceduresProcess> ProceduresProcesses { get; set; }
        public DbSet<Speciality> Specialities { get; set; }

        public DbSet<AreaModule> AreaModules { get; set; }
        public DbSet<ErpManual> ErpManuals { get; set; }
        public DbSet<Bim> Bims { get; set; }

        public DbSet<Blueprint> Blueprints { get; set; }

        public DbSet<BlueprintType> BlueprintTypes { get; set; }
        public DbSet<BluePrintResponsible> BluePrintResponsibles { get; set; }
        public DbSet<BlueprintFolding> BlueprintFoldings { get; set; }

        public DbSet<BluePrintFoldingDetail> BluePrintFoldingDetails { get; set; }
        public DbSet<TechnicalSpec> TechnicalSpecs { get; set; }



        public DbSet<SpecsFormula> SpecFormulas { get; set; }
        public DbSet<TechnicalSpecSpeciality> TechnicalSpecSpecialities { get; set; }
        public DbSet<MixDesign> MixDesigns { get; set; }
        public DbSet<ProviderCatalog> ProviderCatalogs { get; set; }
        public DbSet<TechnicalLibrary> TechnicalLibrarys { get; set; }
        public DbSet<TechnicalVersion> TechnicalVersions { get; set; }
        public DbSet<DesignType> DesignTypes { get; set; }
        public DbSet<CementType> CementTypes { get; set; }
        public DbSet<AggregateType> AggregateTypes { get; set; }
        public DbSet<ConcreteUse> ConcreteUses { get; set; }






        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetType> BudgetTypes { get; set; }
        public DbSet<BudgetInput> BudgetInputs { get; set; }
        public DbSet<BudgetInputAllocation> BudgetInputAllocations { get; set; }
        public DbSet<BudgetInputAllocationGroup> BudgetInputAllocationGroups { get; set; }
        public DbSet<BudgetTitle> BudgetTitles { get; set; }
        public DbSet<ProjectBudgetCategory> ProjectBudgetCategories { get; set; }
        public DbSet<ProjectBudgedItem> ProjectBudgedItems { get; set; }
        public DbSet<SewerBox> SewerBoxes { get; set; }
        public DbSet<SewerManifold> SewerManifolds { get; set; }
        public DbSet<SewerManifoldLetter> SewerManifoldLetters { get; set; }
        public DbSet<SewerManifoldReference> SewerManifoldReferences { get; set; }
        public DbSet<SewerManifoldCostPerformance> SewerManifoldCostPerformances { get; set; }
        public DbSet<SewerManifoldCostPerformanceSewerGroup> SewerManifoldCostPerformanceSewerGroups { get; set; }
        public DbSet<SewerGroup> SewerGroups { get; set; }
        public DbSet<SewerGroupPeriod> SewerGroupPeriods { get; set; }
        public DbSet<SewerGroupProjectHabilitation> SewerGroupProjectHabilitations { get; set; }
        public DbSet<SewerLine> SewerLines { get; set; }
        public DbSet<SewerBoxFootage> SewerBoxFootages { get; set; }
        public DbSet<SewerBoxFootageItem> SewerBoxFootageItems { get; set; }
        public DbSet<FoldingF7> FoldingF7s { get; set; }
        public DbSet<OCBudget> OCBudgets { get; set; }
        public DbSet<ExpensesUtility> ExpensesUtilities { get; set; }
        public DbSet<ConsolidatedBudget> ConsolidatedBudgets { get; set; }
        public DbSet<GoalBudgetInput> GoalBudgetInputs { get; set; }
        public DbSet<ConsolidatedBudgetInput> ConsolidatedBudgetInputs { get; set; }
        public DbSet<GeneralExpense> GeneralExpenses { get; set; }
        public DbSet<SteelVariable> SteelVariables { get; set; }
        public DbSet<Steel> Steels { get; set; }
        public DbSet<MeteredsRestatedByPartida> MeteredsRestatedByPartidas { get; set; }

        public DbSet<MeteredsRestatedByStreetch> MeteredsRestatedByStreetchs { get; set; }
        public DbSet<ConsolidatedSteel> ConsolidatedSteels { get; set; }
        public DbSet<FoldingMeteredsRestatedByPartida> FoldingMeteredsRestatedByPartidas { get; set; }
        public DbSet<ConsolidatedAmountSteel> ConsolidatedAmountSteels { get; set; }
        public DbSet<EntibadoVariable> EntibadoVariables { get; set; }
        public DbSet<Cement> Cements { get; set; }
        public DbSet<CementVariable> CementVariables { get; set; }
        public DbSet<ConsolidatedCement> ConsolidatedCements { get; set; }
        public DbSet<ConsolidatedAmountCement> ConsolidatedAmountCements { get; set; }
        public DbSet<Entibado> Entibados { get; set; }
        public DbSet<ConsolidatedEntibado> ConsolidatedEntibados { get; set; }
        public DbSet<ConsolidatedAmountEntibado> ConsolidatedAmountEntibados { get; set; }
        public DbSet<DiverseInput> DiverseInputs { get; set; }
        #endregion

        #region Production
        public DbSet<RdpItem> RdpItems { get; set; }
        public DbSet<RdpItemFootage> RdpItemFootages { get; set; }
        public DbSet<RdpReport> RdpReports { get; set; }
        public DbSet<RdpItemAccumulatedAmmount> RdpItemAccumulatedAmmounts { get; set; }
        public DbSet<ProductionDailyPart> ProductionDailyParts { get; set; }
        public DbSet<SewerGroupSchedule> SewerGroupSchedules { get; set; }
        public DbSet<SewerGroupScheduleActivity> SewerGroupScheduleActivities { get; set; }
        public DbSet<SewerGroupScheduleActivityDaily> SewerGroupScheduleActivityDailies { get; set; }
        public DbSet<WeeklyAdvance> WeeklyAdvances { get; set; }
        public DbSet<FoldingBudgetWeeklyAdvance> FoldingBudgetWeeklyAdvances { get; set; }
        #endregion

        #region Warehouse
        public DbSet<Stock> Stocks { get; set; }
        /*
        public DbSet<StockApplicationUser> StockApplicationUsers { get; set; }
        public DbSet<StockVoucher> StockVouchers { get; set; }
        public DbSet<StockVoucherDetail> StockVoucherDetails { get; set; }
        public DbSet<StockRoof> StockRoofs { get; set; }
        */
        public DbSet<WarehouseType> WarehouseTypes { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseResponsible> WarehouseResponsibles { get; set; }
        public DbSet<SupplyEntry> SupplyEntries { get; set; }
        public DbSet<SupplyEntryItem> SupplyEntryItems { get; set; }
        public DbSet<FieldRequest> FieldRequests { get; set; }
        public DbSet<FieldRequestFolding> FieldRequestFoldings { get; set; }
        public DbSet<FieldRequestProjectFormula> FieldRequestProjectFormulas { get; set; }
        public DbSet<ReEntryForReturn> ReEntryForReturns { get; set; }
        public DbSet<ReEntryForReturnItem> ReEntryForReturnItems { get; set; }
        #endregion

        #region Accounting
        public DbSet<Invoice> Invoices { get; set; }
        #endregion

        #region Temporal
        public DbSet<ForTest> ForTests { get; set; }
        #endregion

        public IvcDbContext(DbContextOptions<IvcDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<BudgetInputAllocationGroup>(budgetInputAllocationGroup =>
            {
                budgetInputAllocationGroup.HasKey(b => new { b.BudgetInputAllocationId, b.SewerGroupId });

                budgetInputAllocationGroup.HasOne(b => b.SewerGroup)
                    .WithMany(b => b.BudgetInputAllocationGroups)
                    .HasForeignKey(b => b.SewerGroupId)
                    .IsRequired();

                budgetInputAllocationGroup.HasOne(b => b.BudgetInputAllocation)
                    .WithMany(b => b.BudgetInputAllocationGroups)
                    .HasForeignKey(b => b.BudgetInputAllocationId)
                    .IsRequired();
            });

            #region SewerBox
            builder.Entity<SewerBox>()
                .HasIndex(u => new { u.ProjectId, u.Code, u.ProcessType });

            //builder.Entity<SewerBox>(sewerBox =>
            //{
            //    sewerBox.HasMany(s => s.InitialSewerLines)
            //        .WithOne(s => s.InitialSewerBox)
            //        .HasForeignKey(s => s.InitialSewerBoxId)
            //        .IsRequired();

            //    sewerBox.HasMany(s => s.FinalSewerLines)
            //        .WithOne(s => s.FinalSewerBox)
            //        .HasForeignKey(s => s.FinalSewerBoxId)
            //        .IsRequired();
            //});
            #endregion


            //builder.Entity<SewerGroup>(sewerGroup =>
            //{
            //    sewerGroup.HasMany(s => s.Workers)
            //        .WithOne(s => s.SewerGroup)
            //        .HasForeignKey(s => s.SewerGroupId);

            //    sewerGroup.HasOne(s => s.ForemanEmployee)
            //        .WithMany(s => s.SewerGroupsInCharge)
            //        .HasForeignKey(s => s.ForemanEmployeeId);

            //    sewerGroup.HasOne(s => s.ForemanWorker)
            //        .WithMany(s => s.SewerGroupsInCharge)
            //        .HasForeignKey(s => s.ForemanWorkerId);
            //});

            builder.Entity<LetterInterestGroup>(letterInterestGroup =>
            {
                letterInterestGroup.HasKey(l => new { l.LetterId, l.InterestGroupId });
                letterInterestGroup.HasOne(l => l.Letter)
                    .WithMany(l => l.LetterInterestGroups)
                    .HasForeignKey(l => l.LetterId);
                letterInterestGroup.HasOne(l => l.InterestGroup)
                    .WithMany(l => l.LetterInterestGroups)
                    .HasForeignKey(l => l.InterestGroupId);
            });

            builder.Entity<LetterIssuerTarget>(letterIssuerTarget =>
            {
                letterIssuerTarget.HasKey(l => new { l.LetterId, l.IssuerTargetId });
                letterIssuerTarget.HasOne(l => l.Letter)
                    .WithMany(l => l.LetterIssuerTargets)
                    .HasForeignKey(l => l.LetterId);
                letterIssuerTarget.HasOne(l => l.IssuerTarget)
                    .WithMany(l => l.LetterIssuerTargets)
                    .HasForeignKey(l => l.IssuerTargetId);
            });

            builder.Entity<LetterStatus>(letterStatus =>
            {
                letterStatus.HasKey(l => new { l.LetterId, l.LetterDocumentCharacteristicId });
                letterStatus.HasOne(l => l.Letter)
                    .WithMany(l => l.LetterStatus)
                    .HasForeignKey(l => l.LetterId);
            });

            builder.Entity<LetterLetter>(letterLetter =>
            {
                letterLetter.HasKey(l => new { l.LetterId, l.ReferenceId });
            });

            builder.Entity<Letter>(letter =>
            {
                letter.HasMany(l => l.References)
                    .WithOne(l => l.Letter)
                    .HasForeignKey(l => l.LetterId);
                letter.HasMany(l => l.ReferencedBy)
                    .WithOne(l => l.Reference)
                    .HasForeignKey(l => l.ReferenceId);
            });

            builder.Entity<RequestUser>(requestUser =>
            {
                requestUser.HasKey(x => new { x.UserId, x.RequestId });
                requestUser.HasOne(x => x.User)
                    .WithMany(x => x.RequestUsers)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationUserInterestGroup>(userInterestGroup =>
            {
                userInterestGroup.HasKey(u => new { u.UserId, u.InterestGroupId });
                userInterestGroup.HasOne(u => u.User)
                    .WithMany(u => u.UserInterestGroups)
                    .HasForeignKey(u => u.UserId);
                userInterestGroup.HasOne(u => u.InterestGroup)
                    .WithMany(u => u.UserInterestGroups)
                    .HasForeignKey(u => u.InterestGroupId);
            });

            builder.Entity<ApplicationUserProject>(userProject =>
            {
                userProject.HasKey(u => new { u.UserId, u.ProjectId });
                userProject.HasOne(u => u.User)
                    .WithMany(u => u.UserProjects)
                    .HasForeignKey(u => u.UserId);
                userProject.HasOne(u => u.Project)
                    .WithMany(u => u.UserProjects)
                    .HasForeignKey(u => u.ProjectId);
            });

            var modelEntityTypes = builder.Model.GetEntityTypes();
            foreach (var modelEntityType in modelEntityTypes)
            {
                foreach (var foreignKey in modelEntityType.GetForeignKeys())
                {
                    if (!foreignKey.IsOwnership && foreignKey.DeleteBehavior == DeleteBehavior.Cascade)
                    {
                        foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                    }
                }
            }

            builder.Entity<SewerLine>()
                .HasOne(x => x.CompactionDensityCertificate)
                .WithOne(x => x.SewerLine)
                .HasForeignKey<CompactionDensityCertificate>(x => x.SewerLineId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CompactionDensityCertificate>()
                .HasMany(x => x.CompactionDensityCertificateDetails)
                .WithOne(x => x.CompactionDensityCertificate)
                .HasForeignKey(x => x.CompactionDensityCertificateId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<PayrollWorkerVariable>()
                .Property(nameof(PayrollWorkerVariable.Id))
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Entity<PayrollMovementDetail>()
                .Property(nameof(PayrollMovementDetail.Id))
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Entity<WorkFrontProjectPhase>(workFrontProjectPhase =>
            {
                workFrontProjectPhase.HasKey(e => new { e.WorkFrontId, e.ProjectPhaseId });

                workFrontProjectPhase.HasOne(e => e.WorkFront)
                    .WithMany(e => e.ProjectPhases)
                    .HasForeignKey(e => e.WorkFrontId)
                    .IsRequired();

                workFrontProjectPhase.HasOne(e => e.ProjectPhase)
                    .WithMany(e => e.WorkFronts)
                    .HasForeignKey(e => e.ProjectPhaseId)
                    .IsRequired();
            });

            builder.Entity<WorkFrontSewerGroup>(workFrontSewerGroup =>
            {
                workFrontSewerGroup.HasKey(e => new { e.WorkFrontId, e.SewerGroupPeriodId });

                workFrontSewerGroup.HasOne(e => e.WorkFront)
                    .WithMany(e => e.SewerGroupPeriods)
                    .HasForeignKey(e => e.WorkFrontId)
                    .IsRequired();

                workFrontSewerGroup.HasOne(e => e.SewerGroupPeriod)
                    .WithMany(e => e.WorkFronts)
                    .HasForeignKey(e => e.SewerGroupPeriodId)
                    .IsRequired();
            });

            builder.Entity<Provider>(p =>
            {
                p.HasMany(e => e.SupplyGroups)
                    .WithOne(e => e.Provider)
                    .HasForeignKey(e => e.ProviderId);

                p.HasMany(e => e.SupplyFamilies)
                    .WithOne(e => e.Provider)
                    .HasForeignKey(e => e.ProviderId);
            });

            //Dashboard USPS
            builder.Entity<UspWorkersByWeek>().HasNoKey().ToView(null);
            builder.Entity<UspRacsByDay>().HasNoKey().ToView(null);
            builder.Entity<UspHoursByWeek>().HasNoKey().ToView(null);
            builder.Entity<UspCostsByWeek>().HasNoKey().ToView(null);
            builder.Entity<UspBonds>().HasNoKey().ToView(null);
            builder.Entity<UspBondsAll>().HasNoKey().ToView(null);

            //General USPS
            builder.Entity<UspWorkFront>().HasNoKey().ToView(null);

            //RRHH USPs
            builder.Entity<UspWorker>().HasNoKey().ToView(null);
            builder.Entity<UspWorkerExport>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollInvoiceWorker>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollInvoiceDetail>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollInvoiceVariable>().HasNoKey().ToView(null);
            builder.Entity<UspWorkerInvoiceSend>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollWorkerVariable>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollWorkerConcept>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollWorkerInfoAndDailyTask>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollReportPhaseHoursAndCostsByWeek>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollReportPhaseWorkersByWeek>().HasNoKey().ToView(null);
            builder.Entity<UspWorkerDailyTask>().HasNoKey().ToView(null);
            builder.Entity<UspWorkerDailyTasksByDate>().HasNoKey().ToView(null);
            builder.Entity<UspWorkersWeeklyTask>().HasNoKey().ToView(null);
            builder.Entity<UspWorkersWeeklyTaskSummary>().HasNoKey().ToView(null);
            builder.Entity<UspWorkerPayrollDetail>().HasNoKey().ToView(null);
            builder.Entity<UspWorkerInvoiceWokerInfo>().HasNoKey().ToView(null);
            builder.Entity<UspWorkerInvoiceConcept>().HasNoKey().ToView(null);
            builder.Entity<UspWorkerPayrollPension>().HasNoKey().ToView(null);
            builder.Entity<UspWorkerPayrollPensionReport>().HasNoKey().ToView(null);
            builder.Entity<UspWorkersSunatTreg>().HasNoKey().ToView(null);
            builder.Entity<UspWorkersSunatPlame>().HasNoKey().ToView(null);
            builder.Entity<UspWorkersSunatPlameJor>().HasNoKey().ToView(null);
            builder.Entity<UspWorkersSunatPlameReport>().HasNoKey().ToView(null);
            builder.Entity<UspWorkerImport>().HasNoKey().ToView(null);


            builder.Entity<UspListPayroll>().HasNoKey().ToView(null);
            builder.Entity<UspListPayrollAll>().HasNoKey().ToView(null);
            builder.Entity<UspListVariable>().HasNoKey().ToView(null);
            builder.Entity<UspWeekTask>().HasNoKey().ToView(null);
            builder.Entity<UspWeekTaskReportWorkforceHours>().HasNoKey().ToView(null);
            builder.Entity<UspListWeekVariable>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollConceptInvoice>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollPreviousTaxes>().HasNoKey().ToView(null);
            builder.Entity<UspWorkersCovidList>().HasNoKey().ToView(null);
            builder.Entity<UspWorkersCovidCheckList>().HasNoKey().ToView(null);
            builder.Entity<UspWorkersInvoiceList>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollReportWorkforceCost>().HasNoKey().ToView(null);
            builder.Entity<UspPayrollReportPhaseCost>().HasNoKey().ToView(null);

            //Licitaciones USPs
            builder.Entity<UspLegalDocumentations>().HasNoKey().ToView(null);
            builder.Entity<UspSkills>().HasNoKey().ToView(null);
            builder.Entity<UspProfessionalExperienceNumber>().HasNoKey().ToView(null);
            builder.Entity<UspProfessional>().HasNoKey().ToView(null);
            builder.Entity<UspBusinessLegalAgent>().HasNoKey().ToView(null);
            builder.Entity<UspBusiness>().HasNoKey().ToView(null);
            builder.Entity<UspBusinessResponsible>().HasNoKey().ToView(null);
            builder.Entity<UspProfessionalSecondExcel>().HasNoKey().ToView(null);
            builder.Entity<UspProfessionalFirstExcel>().HasNoKey().ToView(null);

            //Equipos USPs

            builder.Entity<UspTransportInsuranceFolding>().HasNoKey().ToView(null);
            builder.Entity<UspTransportSoatFolding>().HasNoKey().ToView(null);
            builder.Entity<UspTransportTechFolding>().HasNoKey().ToView(null);

            builder.Entity<UspMachInsuranceFolding>().HasNoKey().ToView(null);
            builder.Entity<UspMachSoatFolding>().HasNoKey().ToView(null);
            builder.Entity<UspMachTechFolding>().HasNoKey().ToView(null);

            builder.Entity<UspEquipmentVerification>().HasNoKey().ToView(null);
            builder.Entity<UspSupplyVerification>().HasNoKey().ToView(null);
            builder.Entity<UspEquipmentMachineryResponsibles>().HasNoKey().ToView(null);
            builder.Entity<UspEquipmentMachinerySoft>().HasNoKey().ToView(null);
            builder.Entity<UspEquipmentMachinerySoftParts>().HasNoKey().ToView(null);
            builder.Entity<UspEquipmentMachineryTransportInsuranceFolding>().HasNoKey().ToView(null);
            builder.Entity<UspEquipmentMachineryTransport>().HasNoKey().ToView(null);
            builder.Entity<UspEquipmentMachineryTransportPart>().HasNoKey().ToView(null);
            builder.Entity<UspMachineryPart>().HasNoKey().ToView(null);

            //--

            builder.Entity<UspEquipmentMachApp>().HasNoKey().ToView(null);
            builder.Entity<UspEquipmentMachPartApp>().HasNoKey().ToView(null);

            //--

            builder.Entity<UspEquipmentProvider>().HasNoKey().ToView(null);
            builder.Entity<UspFuelTransport>().HasNoKey().ToView(null);
            builder.Entity<UspFuelTransportFolding>().HasNoKey().ToView(null);
            builder.Entity<UspFuelMach>().HasNoKey().ToView(null);
            builder.Entity<UspMachValCustom>().HasNoKey().ToView(null);
            builder.Entity<UspFuelMachFolding>().HasNoKey().ToView(null);
            builder.Entity<UspEquipmentTransportVal>().HasNoKey().ToView(null);

            builder.Entity<UspFuelTransportVal>().HasNoKey().ToView(null);
            builder.Entity<UspFuelTransportValDetail>().HasNoKey().ToView(null);
            builder.Entity<UspFuelTransportValDetailExcel>().HasNoKey().ToView(null);

            builder.Entity<UspRateConsumeTransport>().HasNoKey().ToView(null);
            builder.Entity<UspRateConsumeMach>().HasNoKey().ToView(null);

            builder.Entity<UspFuelMachVal>().HasNoKey().ToView(null);
            builder.Entity<UspFuelMachValDetail>().HasNoKey().ToView(null);


            builder.Entity<UspFuelDates>().HasNoKey().ToView(null);

            builder.Entity<UspTransportCostWork>().HasNoKey().ToView(null);
            builder.Entity<UspTransportCostWorkNonDetail>().HasNoKey().ToView(null);

            builder.Entity<UspMachCostWork>().HasNoKey().ToView(null);
            builder.Entity<UspMachCostWorkNonDetail>().HasNoKey().ToView(null);

            //builder.Entity<UspTransportCostWork>().HasNoKey().ToView(null);
            //builder.Entity<UspTransportCostWorkNonDetail>().HasNoKey().ToView(null);

            builder.Entity<UspEquipmentMachVal>().HasNoKey().ToView(null);
            builder.Entity<UspMachValTable>().HasNoKey().ToView(null);

            builder.Entity<UspEquipmentMach>().HasNoKey().ToView(null);

            builder.Entity<UspEquipmentTransportDetailFolding>().HasNoKey().ToView(null);
            builder.Entity<UspEquipmentMachDetailFolding>().HasNoKey().ToView(null);
            builder.Entity<UspTransportCounts>().HasNoKey().ToView(null);
            builder.Entity<UspMachCounts>().HasNoKey().ToView(null);
            //Finanzas USPs
            builder.Entity<UspBondsActive>().HasNoKey().ToView(null);
            builder.Entity<UspBondAddProjectResponsible>().HasNoKey().ToView(null);

            //Calidad USPs
            builder.Entity<UspEquipmentCertificates>().HasNoKey().ToView(null);
            builder.Entity<UspEquipmentResponsibles>().HasNoKey().ToView(null);
            builder.Entity<UspPatternCalibrations>().HasNoKey().ToView(null);

            //Seguridad USPs
            builder.Entity<UspRacs>().HasNoKey().ToView(null);
            builder.Entity<UspRacsH>().HasNoKey().ToView(null);
            builder.Entity<UspRacsAll>().HasNoKey().ToView(null);
            builder.Entity<UspRacsSCQ>().HasNoKey().ToView(null);
            builder.Entity<UspRacsSAQ>().HasNoKey().ToView(null);
            builder.Entity<UspRacsSewegroup>().HasNoKey().ToView(null);
            builder.Entity<UspTrainingGetCollaborators>().HasNoKey().ToView(null);

            //Control Documentario
            builder.Entity<UspAdvancedSearch>().HasNoKey().ToView(null);
            builder.Entity<UspLetterResponsibles>().HasNoKey().ToView(null);
            builder.Entity<UspLetterReference>().HasNoKey().ToView(null);
            builder.Entity<UspLetter>().HasNoKey().ToView(null);
            builder.Entity<UspPermissionProjectResponsible>().HasNoKey().ToView(null);
            builder.Entity<UspPermissions>().HasNoKey().ToView(null);

            //Oficina Técnica
            builder.Entity<UspBluePrintsApp>().HasNoKey().ToView(null);
            builder.Entity<UspBluePrintResponsibles>().HasNoKey().ToView(null);
            builder.Entity<UspBluePrintReport>().HasNoKey().ToView(null);
            builder.Entity<UspTechnicalSpec>().HasNoKey().ToView(null);
            builder.Entity<UspSpecFormula>().HasNoKey().ToView(null);
            builder.Entity<UspProcedureProcess>().HasNoKey().ToView(null);
            builder.Entity<UspBlueprint>().HasNoKey().ToView(null);
            builder.Entity<UspDiverseInput>().HasNoKey().ToView(null);

            //Almacen

            builder.Entity<UspFieldRequest>().HasNoKey().ToView(null);
            builder.Entity<UspFieldRequestReport>().HasNoKey().ToView(null);
            builder.Entity<UspFieldRequestFoldingReport>().HasNoKey().ToView(null);


            //lo que hizo omar
            builder.Entity<UspMeteredsRestatedByStretch>().HasNoKey().ToView(null);


            builder.Entity<UspSewerGroup>().HasNoKey().ToView(null);
            builder.Entity<UspSewerManifold>().HasNoKey().ToView(null);
            builder.Entity<UspSewerManifoldReview>().HasNoKey().ToView(null);
            builder.Entity<UspSewerManifoldExecution>().HasNoKey().ToView(null);

            //Producción
            builder.Entity<UspProductionDailyPart>().HasNoKey().ToView(null);
            builder.Entity<UspWeeklyAdvanceTotalWorker>().HasNoKey().ToView(null);
            builder.Entity<UspWeeklyAdvanceTotalWorkerCost>().HasNoKey().ToView(null);
        }
    }
}
