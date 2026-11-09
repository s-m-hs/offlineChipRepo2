using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class VahedDTO
    {
        public virtual Guid? ID { get; set; }
        public required string Text { get; set; }

        public required string Code { get; set; }
    }
}
