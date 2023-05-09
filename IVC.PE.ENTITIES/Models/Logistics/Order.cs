using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class Order
    {
        public Guid Id { get; set; }

        public int CorrelativeCode { get; set; }

        public int Type { get; set; }

        public Guid ProviderId { get; set; }

        public Provider Provider { get; set; }

        public Guid? WarehouseId { get; set; }
        public Warehouse.Warehouse Warehouse { get; set; }

        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }

        public string QuotationNumber { get; set; }

        public DateTime Date { get; set; }

        public DateTime? ReviewDate { get; set; }

        public DateTime? ApproveDate { get; set; }

        public int Currency { get; set; }

        public string PaymentMethod { get; set; }

        public string DeliveryTime { get; set; }

        public string BillTo { get; set; }

        public string Warranty { get; set; }

        public string Observations { get; set; }

        public string Conditions { get; set; }

        public string IssuedUserId { get; set; }

        public Uri PriceFileUrl { get; set; }

        public Uri SupportFileUrl { get; set; }

        public Uri PdfFileUrl { get; set; }

        public int Status { get; set; } = ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED;

        public int AttentionStatus { get; set; } = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;

        public bool QualityCertificate { get; set; }
        public bool Blueprint { get; set; }
        public bool SecurityDocument { get; set; }
        public bool CalibrationCertificate { get; set; }
        public bool CatalogAndStorageCriteria { get; set; }
        public bool Other { get; set; }
        public string OtherDescription { get; set; }
        public double Parcial { get; set; }
        public double ExchangeRate { get; set; }
        public IEnumerable<RequestsInOrder> Requests { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public string ClosureReason { get; set; }

        public string CorrelativeCodeSuffix { get; set; }

        public string ManualWarehouse { get; set; }
    }
}
