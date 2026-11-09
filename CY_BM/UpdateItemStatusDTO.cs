using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class UpdateItemStatusDTO
    {
        public Guid ItemID { get; set; }
        public required string StatusText { get; set; }
    }
}

