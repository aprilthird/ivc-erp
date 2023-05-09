using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspEquipmentMach
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectAbbreviation { get; set; }
        public Guid EquipmentProviderId { get; set; }
        public string TradeName { get; set; }
        public Guid EquipmentProviderFoldingId { get; set; }
        public Guid EquipmentMachineryTypeTypeId { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Potency { get; set; }
        public string Year { get; set; }
        public string Plate { get; set; }
        public string SerieNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateString => StartDate.HasValue ? StartDate.Value.ToDateString() : string.Empty;
        public DateTime? LastStartDateInsurance { get; set; }
        public string LastStartDateInsuranceString => LastStartDateInsurance.HasValue ? LastStartDateInsurance.Value.ToDateString() : string.Empty;
        public DateTime? LastEndDateInsurance { get; set; }
        public string LastEndDateInsuranceString => LastEndDateInsurance.HasValue ? LastEndDateInsurance.Value.ToDateString() : string.Empty;
        public DateTime? LastStartDateSoat { get; set; }
        public string LastStartDateSoatString => LastStartDateSoat.HasValue ? LastStartDateSoat.Value.ToDateString() : string.Empty;
        public DateTime? LastEndDateSoat { get; set; }
        public string LastEndDateSoatString => LastEndDateSoat.HasValue ? LastEndDateSoat.Value.ToDateString() : string.Empty;
        public DateTime? LastStartDateTechnical { get; set; }
        public string LastStartDateTechnicalString => LastStartDateTechnical.HasValue ? LastStartDateTechnical.Value.ToDateString() : string.Empty;
        public DateTime? LastEndDateTechnical { get; set; }
        public string LastEndDateTechnicalString => LastEndDateTechnical.HasValue ? LastEndDateTechnical.Value.ToDateString() : string.Empty;
        public int InsuranceNumber { get; set; }
        public int SoatNumber { get; set; }
        public int TecNumber { get; set; }
        public string Cubing { get; set; }
        public int? LastValidityInsurance { get; set; }
        public int? LastValiditySoat { get; set; }
        public int? LastValidityTec { get; set; }
        public int Status { get; set; }
        public string StatusDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_STATUS[Status];
        public int ServiceCondition { get; set; }
        public string ServiceConditionDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_SERVICE_CONDITION[ServiceCondition];
        public double UnitPrice { get; set; }
        public Guid? InsuranceEntityId { get; set; }
        public string LastInsuranceNameDesc { get; set; }
        public string LastInsuranceNumber { get; set; }

        public double? InitHorometer { get; set; }

        public double? EndHorometer { get; set; }

        public double? Dif { get; set; }
    }
}
