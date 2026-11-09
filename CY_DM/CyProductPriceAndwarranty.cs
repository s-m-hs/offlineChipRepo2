using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyProductPriceAndWarranty : BaseDM
    {

        [Display(Name = "قیمت واحد")]
        public int? UnitPrice { get; set; }

        [Display(Name = "قیمت  بدون تخفیف واحد")]
        public int? NoOffUnitPrice { get; set; }


        public required string Title { get; set; }
        public int Availability { get; set; }
        public Guid CyProductId { get; set; }
        // Navigation property
        public virtual CyProduct? CyProduct { get; set; }
    }
}