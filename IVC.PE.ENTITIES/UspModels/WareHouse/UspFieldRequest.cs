using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.WareHouse
{
    [NotMapped]
   public class UspFieldRequest
    {
        public Guid Id { get; set; }

        public string FormulaCodes { get; set; }
        public int DocumentNumber { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int Status { get; set; }
        public string Groups { get; set; }

        public Guid BudgetTitleId { get; set; }

        public string BudgetTitleName { get; set; }


        public Guid ProjectId { get; set; }
        public Guid WorkFrontId { get; set; }
        public string WorkFrontCode { get; set; }
        public Guid? SewerGroupId { get; set; }
        public string SewerGroupCode { get; set; }
        public Guid? WorkFrontHeadId { get; set; }
        public string WorkFrontHeadCode { get; set; }
        public string UserName { get; set; }
        public string MiddleName { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }

        public string IssuedUserId { get; set; }

        public string WorkOrder { get; set; }

        public string Observation { get; set; }

    }
}
