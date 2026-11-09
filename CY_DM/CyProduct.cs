using CY_BM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    //public class CyProduct : BaseDM
    //{
    //    public string Name { get; set; }
    //    public string Description { get; set; }
     
    //    public int QuantityInStock { get; set; }
    //    public ProductStatus Status { get; set; }

    //}

    public class CyProduct : BaseDM
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        public double? Price { get; set; }
        public double? NoOffPrice { get; set; }
        public string? PartNo { get; set; }
        public string?  MfrNo{ get; set; }
        public string? ProductCode {  get; set; }
        public string? DatasheetUrl { get; set; }
        // public string? DataSheet { get; set; }
        // public string? RoHS { get; set; }
        public int Supply { get; set; }

        public string? MainImage { get; set; }
        public string? SmallImage { get; set; }
        public string? Images { get; set; }

        [Display(Name = "گروه اصلی ")]
        public Guid? CyCategoryId { get; set; }

        public virtual CyCategory? CyCategory { get; set; }


        public Guid? CyProductCategoryId {get; set; }
        public virtual CyProductCategory? CyProductCategory {get; set;}

        public Guid? CyManufacturerId { get; set; }
        public virtual CyManufacturer? CyManufacturer { get; set; }

        // Navigation properties
       // public virtual ICollection<CyProductPriceAndWarranty>? PricesAndWarranties { get; set; }
        public virtual ICollection<CyProductSpec>? Specifications { get; set; }
        public ProductStatus status { get; set; }

    }

}
