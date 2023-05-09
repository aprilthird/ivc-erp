﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspPayrollInvoiceVariable
    {
        public Guid WorkerId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
}
