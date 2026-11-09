using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyProductCategory : BaseDM
    {
        [Display(Name = "نام نمایشی")]
        public required string Name { get; set; }

        public required string Code { get; set; }

        [Display(Name = "الویت")]
        public int OrderValue { get; set; }

        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        public string? Url { get; set; }
        public int ProductCount { get; set; }

        public Guid? RootId { get; set; }
        [ForeignKey("RootId")]
        public virtual CyProductCategory? RootCategory { get; set; }
        [InverseProperty("RootCategory")]
        public ICollection<CyProductCategory>? Childs { get; set; }

    }

}
