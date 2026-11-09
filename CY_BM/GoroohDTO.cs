using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class GoroohDTO
    {
        public virtual Guid? ID { get; set; }

        public required string Text { get; set; }

        public required string Code { get; set; }

        public Guid? VahedId { get; set; }
}
}
