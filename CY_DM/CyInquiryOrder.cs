using CY_BM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyInquiryOrder : BaseDM
    {

        public Guid? CyUserID { get; set; }
        public virtual CyUser? CyUser { get; set; }

        public Guid? RequestFile { get; set; }
        public Guid? ResponseFile { get; set; }

        /// <summary>
        /// توضیحات سمت مشتری
        /// </summary>
        public string? CreatorComment { get; set; }



        /// <summary>
        /// نوضیحات سمت فروشنده
        /// </summary>
        public string? CommentB { get; set; }



        /// <summary>
        /// FinalizedDate تاریخ ارسال استعلام
        /// </summary>
        public DateTime? ResponseDate { get; set; }

        public string? CreatorName { get; set; }

    }


}
