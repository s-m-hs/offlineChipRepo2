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
    public class UserDTO
    {
        public Guid? ID { get; set; }
        public required string CyUsNm { get; set; }
        // [StringLength(400)]
        public required string CyHsPs { get; set; }

        public UserStatus Status { get; set; }

        public UserType userType { get; set; }

        public Guid? AccessTableID { get; set; }

        public Guid? CyGoroohID { get; set; }

        public Guid? CyVahedID { get; set; }



    }

}
