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
    public class CyPCBOrder : BaseDM
    {

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


        /// <summary>
        /// تاریخ لغو سفارش
        /// </summary>
        public DateTime? CancelationDate { get; set; }


        /// <summary>
        /// کدپروژه
        /// </summary>
        public int OrderCode { get; set; }



        /// <summary>
        /// شماره سفارش
        /// </summary>
        public int? PcbNumber { get; set; }


        [Required]
        public OrderStatus Status { get; set; }
        //public string? StatusText { get; set; }

        public double? Amount { get; set; }
        public int Count { get; set; }
        public double? TotalAmount { get; set; }


        /// <summary>
        /// واحدپول
        /// </summary>
        public Currency Currency { get; set; }

        public string? CreatorName { get; set; }


        /// <summary>
        /// توضیحات سمت مشتری
        /// </summary>
        public string? CreatorComment { get; set; }



        /// <summary>
        /// نوضیحات سمت فروشنده
        /// </summary>
        public string? CommentB { get; set; }

        /// <summary>
        /// زمان تامین
        /// </summary>
        public string? Duration { get; set; }


        /// <summary>
        /// در حال تامین-درحال ارسال-تحویل داده شده <== وضعیت تامین
        /// </summary>
        public string? InSupplyStatus { get; set; }
        public UserType? LevelOk { get; set; }
        public UserType? LevelBOk { get; set; }



        public Guid? CyVahedId { get; set; }
        [JsonIgnore]
        public virtual CyVahed? CyVahed { get; set; }



        public Guid? CyGoroohId { get; set; }
        [JsonIgnore]
        public virtual CyGorooh? CyGorooh { get; set; }


        /// <summary>
        ///  id
        /// </summary>
        public List<Guid> GuidFileList { get; set; }
        //public List<Guid> StackLayerFiles { get; set; }
        //public List<Guid> MIFiles { get; set; }
        public bool? IsMechanizedAssembly { get; set; }
        //public List<Guid> PickAndPlaceFiles { get; set; }
        //public List<Guid> BOMFiles { get; set; }

        //public Guid? CyPcbFormID { get; set; }
        //public virtual CyPcbForm? CyPcbForm { get; set; }
        // public bool? Notification { get; set; }

       // public virtual ICollection<CyOrderItem>? CyOrderItems { get; set; }
        public virtual ICollection<PCBFile> PCBFile { get; set; } = new List<PCBFile>();
        public virtual ICollection<PCBOrderItemApproval> Approvals { get; set; } = new List<PCBOrderItemApproval>();


    }

    public class PCBFile : BaseDM
    {
        public Guid? CyPCBOrderID { get; set; }
        public virtual CyPCBOrder? CyPCBOrder { get; set; }

        public Guid? CyUserID { get; set; }
        public virtual CyUser? CyUser { get; set; }
        public Guid? FileID { get; set; }
        public DateTime? SendDate { get; set; }
        public PCBFileType Type { get; set; }
    }

    public class PCBOrderItemApproval : BaseDM
    {
        public Guid CyPCBOrderID { get; set; }
        [JsonIgnore]
        public virtual CyPCBOrder? CyPCBOrder { get; set; }


        public Guid? CyUserID { get; set; }
        public virtual CyUser? CyUser { get; set; }

        public string? UserName { get; set; }

        public UserType Role { get; set; }

        public string? RoleName { get; set; }

        public bool? IsApproved { get; set; } = false;
        public DateTime? ActionTime { get; set; }
        public string? Comment { get; set; }

        public bool? IsFirst { get; set; } = false;
    }
}
