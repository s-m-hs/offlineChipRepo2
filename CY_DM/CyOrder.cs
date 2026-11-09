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
    public class CyOrder : BaseDM
    {
        public double TotalAmoun;

        public Guid? CyUserID { get; set; }
        public virtual CyUser? CyUser { get; set; }

        //public Ordermode OrderMode { get; set; }

        /// <summary>
        /// تاریخ ایجاد سفارش
        /// </summary>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        ///  تاریخ ارسال سفارش برای استعلام گیری
        /// </summary>
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// FinalizedDate تاریخ ارسال استعلام
        /// </summary>
        public DateTime? InquiryDate { get; set; }

        /// <summary>
        /// تاریخ تایید سفارش
        /// </summary>
        public DateTime? VerifiedDate { get; set; }
        /// <summary>
        /// تاریخ شروع سفارش
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// تاریخ تحویل و پایان سفارش
        /// </summary>
        public DateTime? DeliveredDate { get; set; }
        //public DateTime? InProcessDate { get; set; }
        /// <summary>
        /// تاریخ لغو سفارش
        /// </summary>
        public DateTime? CancelationDate { get; set; }

        public int OrderCode { get; set; }


        public Currency? Curency { get; set; }


        [Required]
        public OrderStatus Status { get; set; }
        //public string? StatusText { get; set; }



        public double? TotalAmount { get; set; }


        public Guid? CyVahedId { get; set; }
        [JsonIgnore]
        public virtual CyVahed? CyVahed { get; set; }



        public Guid? CyGoroohId { get; set; }
        [JsonIgnore]
        public virtual CyGorooh? CyGorooh { get; set; }


        //public Guid? CyPcbFormID { get; set; }
        //public virtual CyPcbForm? CyPcbForm { get; set; }
       // public bool? Notification { get; set; }

        public virtual ICollection<CyOrderItem>? CyOrderItems { get; set; }
    }
}
