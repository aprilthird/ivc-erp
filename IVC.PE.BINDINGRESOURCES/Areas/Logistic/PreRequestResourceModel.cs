using IVC.PE.APP.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.Logistic
{
    public class PreRequestRegisterResourceModel
    {


        public Guid ProjectId { get; set; }


        public Guid BudgetTitleId { get; set; }

        public DateTime DeliveryDate { get; set; }


        //public Guid? SupplyFamilyId { get; set; }


        public Guid? ProjectFormulaId { get; set; }


        public int RequestType { get; set; }


        public string Observations { get; set; }

        public string IssuedUserId { get; set; }

        //public IEnumerable<PreRequestUser> PreRequestUsers { get; set; }

        //public IEnumerable<PreRequestItem> PreRequestItems { get; set; }
    }
    public class PreRequestEditResourceModel
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }


        public Guid BudgetTitleId { get; set; }


        public Guid? SupplyFamilyId { get; set; }


        public Guid? ProjectFormulaId { get; set; }

        public DateTime DeliveryDate { get; set; }


        public int RequestType { get; set; }


        public string Observations { get; set; }

        public string IssuedUserId { get; set; }

    }


    public class PreRequestDetailRegisterResourceModel
    {
        public Guid Id { get; set; }

        public Guid PreRequestId { get; set; }

        public Guid? SupplyId { get; set; }

        public Guid WorkFrontId { get; set; }


        public double Measure { get; set; }


        public string UsedFor { get; set; }

        public string Observations { get; set; }

        public bool SupplyManual { get; set; }

        public string SupplyName { get; set; }

        public string MeasurementUnitName { get; set; }
    }

    public class PreRequestDetailListResourceModel
    {
        public Guid Id { get; set; }

        public Guid PreRequestId { get; set; }

        public Guid SupplyId { get; set; }

        public Guid WorkFrontId { get; set; }
        public string WorkFrontCode { get; set; }

        public string Color { get; set; }

        public string SupplyGroupStr { get; set; }

        public string SupplyDescription { get; set; }

        public double Measure { get; set; }
        public string MeasureStr { get; set; }

        public double MeasureInAttention { get; set; }

        public double MeasureToAttent { get; set; }

        public string UsedFor { get; set; }

        public string Observations { get; set; }

        public string Metered { get; set; }

        public string SupplyName { get; set; }

        public string MeasurementUnitName { get; set; }



    }

    public class PreRequestUserRegisterResourceModel
    {
        public Guid Id { get; set; }

        public Guid PreRequestId { get; set; }

        public string UserId { get; set; }

        public string FullName { get; set; }
    }
    public class PreRequestAllListResourceModel
    { public Guid Id { get; set; }

        public string IssuedUserId { get; set; }
        public Guid ProjectId { get; set; }

        public string ProjectCostCenter { get; set; }

        public string ProjectAbbreviation { get; set; }

        public string CorrelativeCodeStr { get; set; }

        public string CorrelativePrefix { get; set; }

        public Guid BudgetTitleId { get; set; }

        public string BudgetName { get; set; }

        public int RequestType { get; set; }

        public string RequestTypeStr => ConstantHelpers.Logistics.RequestOrder.Type.VALUES[RequestType];

        public string RequestDescription { get; set; }

        public int OrderStatus { get; set; }

        public string IssueDate { get; set; }

        public string DeliveryDate { get; set; }

        public string Observations { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public string ProjectFormulaCode { get; set; }

        public string ProjectFormulaName { get; set; }

        public int AttentionStatus { get; set; }



    }
}
