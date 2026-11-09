using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CY_BM;

namespace CY_DM
{
    public class CyProductOnline
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public double? Price { get; set; }
        public double? NoOffPrice { get; set; }
        public string? PartNo { get; set; }
        public string? MfrNo { get; set; }
        public string? ProductCode { get; set; }
        public string? DatasheetUrl { get; set; }
        // public string? DataSheet { get; set; }
        // public string? RoHS { get; set; }
        public int Supply { get; set; }

        public string? MainImage { get; set; }
        public string? SmallImage { get; set; }
        public string? Images { get; set; }

        [Display(Name = "گروه اصلی ")]
        public int? CyCategoryId { get; set; }

        public virtual CyCategory? CyCategory { get; set; }


        public int? CyProductCategoryId { get; set; }
        public virtual CyProductCategory? CyProductCategory { get; set; }

        public int? CyManufacturerId { get; set; }
        public virtual CyManufacturer? CyManufacturer { get; set; }

        // Navigation properties
        public virtual ICollection<CyProductPriceAndWarranty>? PricesAndWarranties { get; set; }
        public virtual ICollection<CyProductSpec>? Specifications { get; set; }
        public ProductStatus status { get; set; }
    }
}
