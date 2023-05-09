using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.LegalTechnicalLibrary;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.IntegratedManagementSystem
{
    public class SewerManifoldFor24FirstPart
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid NewSIGProcessId { get; set; }
        public NewSIGProcess NewSIGProcess { get; set; }
        public string ReportUserId { get; set; }
        public string ReportUserName { get; set; }
        public DateTime Date { get; set; }
        //------------------------Origen Del P/S NC:
        public int OriginType { get; set; }
        public int NCOrigin { get; set; }
        public Guid? SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }
        public Guid? ProviderId { get; set; }
        public Provider Provider { get; set; }
        public string Client { get; set; }
        //------------------------Descripción del Hallazgo:
        public string Description { get; set; }
        //------------------------Si Aplica llene los siguientes datos
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string BrandProvider { get; set; }
        public string CodeReference { get; set; }
        public DateTime ExpirationDate { get; set; }
        //------------------------Responsable en levantar
        public string ResponsableUserId { get; set; }
        public string ResponsableUserName { get; set; }
        //------------------------Regristro fotográfico
        public Uri FileUrl { get; set; }
        public Uri VideoUrl { get; set; }
        public ICollection<For24FirstPartGallery> for24FirstPartGallery { get; set; }
    }
}
