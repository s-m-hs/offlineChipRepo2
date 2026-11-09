using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyProductSpec : BaseDM
    {
        public required string Name { get; set; }
        public required string Value { get; set; }

        public Guid CyProductId { get; set; }
        // Navigation property
        public virtual CyProduct? CyProduct { get; set; }

        public Guid? CyProductCategoryId { get; set; }
        public virtual CyProductCategory? CyProductCategory { get; set; }

    }
}
