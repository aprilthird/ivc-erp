using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using IVC.PE.CORE.Helpers;

namespace IVC.PE.ENTITIES.UspModels.DocumentaryControl
{
    [NotMapped]
    public class UspAdvancedSearch
    {
        public Guid Id { get; set; }

        public int Type { get; set; }

        public Uri FileUrl { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }
    }
}
