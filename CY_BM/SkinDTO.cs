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
   
    public class SkinDTO
    {
        public Guid ID { get; set; }

        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        public required string Code { get; set; }

    }
   
}
