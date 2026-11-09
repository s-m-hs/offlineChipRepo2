using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyManufacturer : BaseDM
    {
        public required string Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation property
        public virtual ICollection<CyProduct>? CyProducts { get; set; }
    }
}
