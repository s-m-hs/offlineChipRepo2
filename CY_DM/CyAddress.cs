using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyAddress:BaseDM
    {
        public Guid? CyUserID { get; set; }
        public virtual CyUser? CyUser { get; set; }
        public required string Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
       
        public string? Phone { get; set; }
        public string? Mobile { get; set; }

    }
}
