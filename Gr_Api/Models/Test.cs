using CY_DM;

namespace Gr_Api.Models
{
    public class GrKe
    {
        public long ID { get; set; }
        //public required global MyProperty { get; set; }
        public required string Key { get; set; }
        public string? Value { get; set; }

        public long? GrProductID { get; set; }
        public virtual GrProduct? GrProduct { get; set; }
    }
}
