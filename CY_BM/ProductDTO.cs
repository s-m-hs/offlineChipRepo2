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
   
    public class ProductDTO
    {
        public Guid ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ProductCode { get; set; }
        public double? Price { get; set; }
        public double? NoOffPrice { get; set; }
        public string? PartNo { get; set; }
        public string? MfrNo { get; set; }
        public string? DatasheetUrl { get; set; }
        public int Supply { get; set; }

        // public string? DataSheet { get; set; }
        // public string? RoHS { get; set; }

        public string? MainImage { get; set; }
        public string? SmallImage { get; set; }
        public string? Images { get; set; }
        public Guid? CyManufacturerId { get; set; }
        public string? Manufacturer {  get; set; }
        [Display(Name = "گروه اصلی ")]
        public Guid? CyCategoryId { get; set; }
        public Guid? CyProductCategoryId {get; set;}

        public List<ProductSpecDTO>? Spec {  get; set; }
        // Navigation properties
        // public virtual ICollection<CyProductPriceAndWarranty>? PricesAndWarranties { get; set; }
        // public virtual ICollection<CyProductSpec>? Specifications { get; set; }

    }
    public class ProductSpecDTO 
    {
        public Guid ID { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
        public Guid CyProductId { get; set; }
        // Navigation property
    }

}
