using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.SewerManifoldFor24ViewModels
{
    public class SewerManifoldFor24ViewModel
    {
        public Guid? Id { get; set; }
        public Guid? SewerManifoldFor24FirstPartId { get; set; }
        public SewerManifoldFor24FirstPartViewModel SewerManifoldFor24FirstPart { get; set; }
        public Guid? SewerManifoldFor24SecondPartId { get; set; }
        public SewerManifoldFor24SecondPartViewModel SewerManifoldFor24SecondPart { get; set; }
        public Guid? SewerManifoldFor24ThirdpartId { get; set; }
        public SewerManifoldFor24ThirdPartViewModel SewerManifoldFor24ThirdPart { get; set; }
    }
}
