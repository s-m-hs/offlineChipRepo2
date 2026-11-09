using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyProfile : BaseDM
    {
        public string? Name { get; set; }
        // [StringLen   gth(40)]
        public string? Family { get; set; }
        // [StringLength(40)]
        public string? Email { get; set; }
        //   [StringLength(40)]
        public string? Website { get; set; }
        //  [StringLength(40)]
        public string? Mobile { get; set; }

        public string? Description { get; set; }

        public string? UserImageUrl { get; set; }


    }
}
