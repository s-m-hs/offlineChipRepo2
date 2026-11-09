using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CY_DM
{
    public class CyGorooh : BaseDM
    {
        [Display(Name = "نام نمایشی")]
        public required string Text { get; set; }

        public required string Code { get; set; }

        public Guid VahedId { get; set; }
        [JsonIgnore]

        public virtual CyVahed? Vahed { get; set; }

    }
}
