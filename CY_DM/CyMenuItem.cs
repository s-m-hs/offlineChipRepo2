using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyMenuItem : BaseDM
    {
        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        [Display(Name = "اولیت نمایش")]
        public int OrderValue { get; set; }

        [Display(Name = "آدرس صفحه")]
        public string? PageUrl { get; set; }

        [Display(Name = "گروه")]
        public Guid? CyCategoryId { get; set; }
        public virtual CyCategory? CyCategory { get; set; }

        [Display(Name = "پوسته")]
        public Guid? CySkinId { get; set; }
        public virtual CySkin? CySkin { get; set; }

        public bool? isProduct { get; set; }

        [Display(Name = "والد")]
        public Guid? rootId { get; set; }
        [ForeignKey("rootId")]
        public virtual CyMenuItem? rootMenuItem { get; set; }

        [Display(Name = "منو")]
        public Guid? CyMenuId { get; set; }
        public virtual CyMenu? CyMenu { get; set; }

        public string? Meta { get; set; }
        public string? ImageUrl { get; set; }

        [InverseProperty("rootMenuItem")]
        public virtual ICollection<CyMenuItem>? childItems { get;  }
    }


}
