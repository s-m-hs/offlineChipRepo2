using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class ManufacturerDTO
    {
        public Guid ID { get; set; }
        public required string Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation property
    }
  
}
