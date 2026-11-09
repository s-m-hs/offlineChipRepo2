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
    public class ProfileDTO
    {
        public Guid ID { get; set; }
        public Guid CyUserID { get; set; }
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

        public string? Username { get; set; }
        public int? BasketCount { get; set; }

    }
}
