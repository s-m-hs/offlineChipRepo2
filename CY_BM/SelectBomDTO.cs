using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class SelectBomDTO
    {
        public Guid ID { get; set; }
        public double? Price { get; set; }
        public string? ManufacturerName { get; set; }
        public int Supply {  get; set; }
    }
}
