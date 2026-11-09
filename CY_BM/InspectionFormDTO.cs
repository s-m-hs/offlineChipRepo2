using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CY_BM;

namespace CY_BM
{
    public class InspectionFormDTO
    {
        public Guid UserID { get; set; }
        public InspectionLab? Lab { get; set; }
        public ICollection<InspectionItemDTO>? Items { get; set; }
        public int? ExternalVisualInspection { get; set; }
        public int? PinCorrelationTest { get; set; }
        public int? ProgrammingTest { get; set; }
        public int? SolderabilityAnalysis { get; set; }
        public int? Radiography { get; set; }
        public int? XRFTest { get; set; }
        public int? KeyFunctional { get; set; }
        public int? Baking { get; set; }
        public int? TapeAndReel { get; set; }
        public int? InternalVisualInspection { get; set; }
        public int? HeatedChemicalTest { get; set; }
        public Guid? File {  get; set; }
        public Guid? CyAddressID {  get; set; }
    }
}
