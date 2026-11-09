using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class InquiryOrderDTO
    {
        public  Guid? ID { get; set; }


        public  DateTime? CreateDate { get; set; }= DateTime.Now;
        public DateTime? ResponseDate { get; set; }

        public Guid? RequestFile { get; set; }
        public Guid? ResponseFile { get; set; }

        public string? CreatorComment { get; set; }

        public string? CommentB { get; set; }


        public Guid? CyUserID { get; set; }

        public string? CreatorName { get; set; }

    }
}