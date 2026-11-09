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
   
    public class KeyDataDTO
    {
        public Guid? ID { get; set; }
        [Display]
        public string? Key { get; set; }

        public string? Tag { get; set; }

        [Display(Name = "مقدار")]
        public string? Value { get; set; }

        public int? IntValue { get; set; }
        public double? DoubleValue { get; set; }

    }
  
}
