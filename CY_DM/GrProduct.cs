using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class GrProduct
    {
        public long ID { get; set; }
        //public required global MyProperty { get; set; }
        public required string PartNumber { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
        public string? Stock { get; set; }
        public string? DatasheetUrl { get; set; }
        public string? SmartImageUrl { get; set; }

        public string Url { get; set; }

        public bool? IsFullyGet { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? LastModified { get; set; }

        public Guid? GrCategoryID { get; set; }
        public virtual GrCategory? GrCategory { get; set; }

        public Guid? GrManufacturerID { get; set; }
        public virtual GrManufacturer? GrManufacturer{get;set;}
       
       



        public virtual ICollection<GrKeyValue>? GrKeyValue {  get; set; }
    }
    public class GrKeyValue
    {
        public long ID { get; set; }
        //public required global MyProperty { get; set; }
        public required string Key { get; set; }
        public string? Value { get; set; }

        public long ? GrProductID { get; set; }
        public virtual GrProduct? GrProduct { get; set; }
    }
    public class GrManufacturer
    {
        public Guid ID { get; set; }
        //public required global MyProperty { get; set; }
        public required string Name { get; set; }
        public string? Image{ get; set; }
        public string? Url { get; set; }


    }
    public class GrCategory
    {
        public Guid ID { get; set; }

        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        public required string Code { get; set; }

        public string? description { get; set; }

        [Display(Name = "الویت")]
        public int OrderValue { get; set; }

        public string? ImageUrl { get; set; }
        public int ProductCount { get; set; }
        public string? Url { get; set; }

        public Guid? rootId { get; set; }

        [ForeignKey("rootId")]
        public virtual CyCategory? rootCategory { get; set; }

    }

}
