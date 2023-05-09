using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.Warehouse
{
    public class FieldRequestResourceModel
    {
        public Guid Id { get; set; }

        public string FormulaCodes { get; set; }
        public string DocumentNumber { get; set; }

        public string DeliveryDate { get; set; }

        public Guid BudgetTitleId { get; set; }

        public string BudgetTitleName { get; set; }


        public Guid ProjectId { get; set; }
        public Guid? WorkFrontId { get; set; }
        public string WorkFrontCode { get; set; }
        public Guid? SewerGroupId { get; set; }
        public string SewerGroupCode { get; set; }
        public Guid? WorkFrontHeadId { get; set; }
        public string WorkFrontHeadCode { get; set; }
        public string UserName { get; set; }
        public string MiddleName { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }

        public int Status { get; set; }

        public string IssuedUserId { get; set; }

        public string WorkOrder { get; set; }

        public string Observation { get; set; }
    }

    public class FieldRequestFatherRegisterResourceModel
    {

        public DateTime DeliveryDate { get; set; }       

        public Guid BudgetTitleId { get; set; }

        public List<Guid> ProjectFormulaIds { get; set; }
        public Guid ProjectId { get; set; }
        public Guid WorkFrontId { get; set; }
        public Guid SewerGroupId { get; set; }

        public string Observation { get; set; }

        public string WorkOrder { get; set; }
        public string IssuedUserId { get; set; }
    }

    public class FieldRequestFatherandItemRegisterResourceModel
    {
        public Guid FieldRequestId { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime DeliveryDate { get; set; }

        public Guid BudgetTitleId { get; set; }

        public Guid ProjectFormulaId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid WorkFrontId { get; set; }
        public Guid SewerGroupId { get; set; }
        public Guid WorkFrontHeadId { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid SupplyFamilyId { get; set; }

        public int Status { get; set; }

        public string Observation { get; set; }

        public string WorkOrder { get; set; }
        public string IssuedUserId { get; set; }

        public Guid FieldRequestFoldingId { get; set; }

        public Guid GoalBudgetInputId { get; set; }

        public Guid ProjectPhaseId { get; set; }

        public string Quantity { get; set; }
    }

    public class FieldRequestFatherFormulaRegisterResourceModel
    {
        public Guid Id { get; set; }

        public Guid ProjectFormulaId { get; set; }
    }


    public class FieldRequestFoldingResourceModel
    {
        public Guid Id { get; set; }

        public int SupplyCorrelativeCode { get; set; }

        public string SupplyFamilyName { get; set; }

        public string SupplyFamilyCode { get; set; }

        public string SupplyGroupName { get; set; }

        public string SupplyGroupCode { get; set; }

        public string SupplyDescription { get; set; }

        public string MeasurementUnitAbbreviation { get; set; }

        public string ProjectPhaseCode { get; set; }
        public string Quantity { get; set; }

    }

    public class FieldRequestFoldingRegisterResourceModel
    {
        public Guid Id { get; set; }

        public Guid FieldRequestId { get; set; }

        public Guid GoalBudgetInputId { get; set; }

        public Guid ProjectPhaseId { get; set; }

        public string Quantity { get; set; }

    }
}
