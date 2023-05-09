﻿using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class ProjectPayrollResponsible
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public string Responsible1Id { get; set; }

        public string Responsible2Id { get; set; }

        public string Responsible3Id { get; set; }
    }
}
