using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyMenu : BaseDM
    {
        public CyMenu()
        {
            CyMenuItems=new List<CyMenuItem>(); 
        }

        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        public required string NameCode { get; set; }

        public string? ImageUrl { get; set; }

        public virtual ICollection<CyMenuItem>? CyMenuItems { get; set; }

    }
}
