using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class ChangePasswordDTO
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}

