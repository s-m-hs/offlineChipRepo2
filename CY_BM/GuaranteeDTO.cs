using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class GuaranteeDTO
    {
        public Guid ID { get; set; }
        public Guid GuaranteeID { get; set; }
        public string? Username { get; set; }
        public string? Phonenumber {  get; set; }
        public string? ProductName { get; set; }
        public string? ProductStatus { get; set; }
        public string? GuaranteeCompany { get; set; }
        public string? GuarantreePrice { get; set; }
        public DateTime RecievedDate { get; set; }
        public GuaranteeType Type { get; set; }
        public string? ProductProblem { get; set; }
        public string? Details { get; set; }
        public string? CompanyExplaination { get; set; }
    }

    public class UpdateGuaranteeDTO
    {
        public Guid ID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductStatus { get; set; }
        public string? GuaranteeCompany { get; set; }
        public string? GuarantreePrice { get; set; }
        public string? ProductProblem { get; set; }
        public string? Details { get; set; }
        public string? CompanyExplaination { get; set; }
    }
}
