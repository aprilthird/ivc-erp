using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class Supply
    {
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }

        public Guid MeasurementUnitId { get; set; }

        public MeasurementUnit MeasurementUnit { get; set; }

        public Guid SupplyFamilyId { get; set; }

        public SupplyFamily SupplyFamily { get; set; }

        public Guid? SupplyGroupId { get; set; }

        public SupplyGroup SupplyGroup { get; set; }

        public int CorrelativeCode { get; set; }

        [NotMapped]
        public string CorrelativeCodeString => CorrelativeCode.ToString("D3");

        [NotMapped]
        public string FullCode => $"{SupplyFamily.Code}{SupplyGroup.Code}{CorrelativeCodeString}";

        public int Status { get; set; }
    }
}
