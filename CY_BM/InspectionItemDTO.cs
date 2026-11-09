using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class InspectionItemDTO
    {
        public string? PartNumber { get; set; }
        public int Quantity { get; set; }
        public string? TargetDate { get; set; }
    }
}