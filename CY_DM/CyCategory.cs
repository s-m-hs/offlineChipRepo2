using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyCategory : BaseDM
    {
        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        public required string Code { get; set; }

        [Display(Name = "الویت")]
        public int OrderValue { get; set; }

        public string? ImageUrl { get; set; }
        public int ProductCount { get; set; }
        [Display(Name = "پوسته")]
        public Guid? CySkinId { get; set; }
        public virtual CySkin? CySkin { get; set; }

        public Guid? rootId { get; set; }

        [ForeignKey("rootId")]
        public virtual CyCategory? rootCategory { get; set; }

        [InverseProperty("rootCategory")]
        public virtual ICollection<CyCategory>? childItems { get; set; }

    }
}
