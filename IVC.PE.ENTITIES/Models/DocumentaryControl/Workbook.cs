using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
    public class Workbook
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public int Number { get; set; }

        public Uri FileUrl { get; set; }
        public string Name { get; set; }

        public string Range { get; set; }

        public string Term { get; set; }
    }
}
