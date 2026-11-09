using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class AddressDTO
    {
        public Guid? ID { get; set; }
        public required string Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
    }

    public class AddressPutDTO
    {
        public Guid CyUserID { get; set; }
        public required string Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
    }
}
