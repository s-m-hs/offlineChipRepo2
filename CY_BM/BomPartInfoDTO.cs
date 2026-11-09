using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class BomPartInfoDTO
    {
        public required string PartNumber { get; set; }
        public string? Manufacturer { get; set; }
        public int Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public double? TotalPrice { get; set; }
        public Guid? CyProductID { get; set; }
    }
}
