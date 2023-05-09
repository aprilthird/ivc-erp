using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.DocumentaryControl
{
    [NotMapped]
    public class UspLetterReference
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string DateStr => Date.ToDateString();
        public string Subject { get; set; }
        public string IssuerName { get; set; }
        public Uri FileUrl { get; set; }
    }
}
