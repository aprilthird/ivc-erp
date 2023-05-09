using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels
{
    public class WorkerWorkPeriodViewModel
    {
        public Guid? Id { get; set; }

        public Guid WorkerId { get; set; }
        public WorkerViewModel Worker { get; set; }


        public DateTime EntryDateDt { get; set; }
        [Display(Name = "Fecha de Ingreso", Prompt = "Fecha de Ingreso")]
        public string EntryDate { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name = "Fondo de Pensión", Prompt = "Fondo de Pensión")]
        public Guid PensionFundAdministratorId { get; set; }
        public PensionFundAdministratorViewModel PensionFundAdministrator { get; set; }
        [Display(Name = "CUSSP", Prompt = "CUSSP")]
        public string PensionFundUniqueIdentificationCode { get; set; }

        [Display(Name = "Categoría", Prompt = "Categoría")]
        public int Category { get; set; }

        [Display(Name = "Procedencia", Prompt = "Procedencia")]
        public int Origin { get; set; }

        [Display(Name = "Destino", Prompt = "Destino")]
        public int Workgroup { get; set; }

        [Display(Name = "Cargo", Prompt = "Cargo")]
        public Guid? WorkerPositionId { get; set; }
        public WorkPositionViewModel WorkerPosition { get; set; }

        [Display(Name = "Nro.Hijos", Prompt = "Nro.Hijos")]
        public int NumberOfChildren { get; set; }

        [Display(Name = "Cuota Sindical", Prompt = "Cuota Sindical")]
        public bool HasUnionFee { get; set; }

        [Display(Name = "Aporta SCTR", Prompt = "Aporta SCTR")]
        public bool HasSctr { get; set; }

        [Display(Name = "SCTR Salud", Prompt = "SCTR Salud")]
        public int SctrHealthType { get; set; }

        [Display(Name = "SCTR Pensión", Prompt = "SCTR Pensión")]
        public int SctrPensionType { get; set; }

        [Display(Name = "Retención Judicial Fija", Prompt = "Retención Judicial Fija")]
        public decimal JudicialRetentionFixedAmmount { get; set; }

        [Display(Name = "Retención Judicial Porcentaje", Prompt = "Retención Judicial Porcentaje")]
        public decimal JudicialRetentionPercentRate { get; set; }

        [Display(Name = "Liquidación Semanal", Prompt = "Liquidación Semanal")]
        public bool HasWeeklySettlement { get; set; }

        [Display(Name = "Régimen Laboral", Prompt = "Régimen Laboral")]
        public int LaborRegimen { get; set; }

        [Display(Name = "Aporta EPS", Prompt = "Aporta EPS")]
        public bool HasEPS { get; set; }

        [Display(Name = "Aporta EsSalud Vida", Prompt = "Aporta EsSalud Vida")]
        public bool HasEsSaludPlusVida { get; set; }

        [Display(Name = "Fecha de cese", Prompt = "Fecha de cese")]
        public string CeaseDate { get; set; }

        [Display(Name = "Estado", Prompt = "Estado")]
        public bool IsActive { get; set; }
    }

    public class WorkerWorkPeriodCeaseViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Fecha de Ingreso", Prompt = "Fecha de Ingreso")]
        public string EntryDate { get; set; }

        [Display(Name = "Fecha de Cese", Prompt = "Fecha de Cese")]
        public string CeaseDate { get; set; }
    }
}
