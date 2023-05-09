using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels
{
    public class SupplyViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Unidad de Medida", Prompt = "Unidad de Medida")]
        public Guid MeasurementUnitId { get; set; }

        public MeasurementUnitViewModel MeasurementUnit { get; set; }

        [Display(Name = "Familia", Prompt = "Familia")]
        public Guid SupplyFamilyId { get; set; }

        public SupplyFamilyViewModel SupplyFamily { get; set; }

        [Display(Name = "Grupo", Prompt = "Grupo")]
        public Guid? SupplyGroupId { get; set; }

        public SupplyGroupViewModel SupplyGroup { get; set; }

        [Display(Name = "Correlativo", Prompt = "Correlativo")]
        public int CorrelativeCode { get; set; }

        public string CorrelativeCodeString => CorrelativeCode.ToString("D3");

        public string FullCode => $"{SupplyFamily?.Code}{SupplyGroup?.Code}{CorrelativeCodeString}";

        public int Status { get; set; }
    }
}
