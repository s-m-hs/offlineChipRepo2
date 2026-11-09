using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyKeyData : BaseDM
    {
        [Display]
        public required string Key { get; set; }


        [Display(Name = "مقدار")]
        public required string Value { get; set; }

        public int? IntValue { get; set; }
        public double? DoubleValue { get; set; }
        public string? Tag { get; set; }

        
 
    }
}
