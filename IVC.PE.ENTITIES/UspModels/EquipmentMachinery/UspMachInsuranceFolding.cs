﻿using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspMachInsuranceFolding
    {
        public string Center { get; set; }
        public string Tradename { get; set; }
        public Guid FoldingId { get; set; }
        public DateTime EndDateInsurance { get; set; }
        public string EndDateInsuranceString => EndDateInsurance.ToDateString();
        public DateTime StartDateInsurance { get; set; }
        public string StartDateInsuranceStr => StartDateInsurance.ToDateString();
        
        public string Number { get; set; }
        public int OrderInsurance { get; set; }
        public Uri InsuranceFileUrl { get; set; }

        public string Model { get; set; }

        public string Brand { get; set; }
        public string SerieNumber { get; set; }

        public int Validity { get; set; }

        public bool Days30 { get; set; }
        public bool Days15 { get; set; }
    }
}
