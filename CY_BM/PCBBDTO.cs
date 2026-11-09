using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CY_BM
{
    public class PCBBDTO 
    {
        public virtual Guid? ID { get; set; }

        public Guid? CyUserID { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? SendDate { get; set; }

        public DateTime? InquiryDate { get; set; }

        public DateTime? VerifiedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DeliveredDate { get; set; }
        //public DateTime? InProcessDate { get; set; }

        public DateTime? CancelationDate { get; set; }

        public int OrderCode { get; set; }
        public int? PcbNumber { get; set; }


        public OrderStatus? Status { get; set; }
        //public string? StatusText { get; set; }

        public double? Amount { get; set; }
        public int Count { get; set; }
        public double? TotalAmount { get; set; }

        /// <summary>
        /// واحدپول
        /// </summary>
        public Currency Currency { get; set; }


        /// <summary>
        /// ایجاد کننده ایتم
        /// </summary>
        /// 
        public Guid? CreatorId { get; set; }
        public string? CreatorName { get; set; }
        public string? CreatorComment { get; set; }
        public string? CommentB { get; set; }
        public string? Duration { get; set; }
        public UserType? LevelOk { get; set; }
        public UserType? LevelBOk { get; set; }
        public string? InSupplyStatus { get; set; }
        public Guid? CyVahedId { get; set; }

        public Guid? CyGoroohId { get; set; }

        public List<Guid> GuidFileList { get; set; }

        public bool? IsMechanizedAssembly { get; set; }

        public virtual ICollection<PCBFileDTO> PCBFileDTO { get; set; } = new List<PCBFileDTO>();
        public virtual ICollection<PCBOrderItemApprovalDTO> Approvals { get; set; } = new List<PCBOrderItemApprovalDTO>();

    }

    public class PCBFileDTO
    {
        public Guid? ID { get; set; }
        public  DateTime CreateDate { get; set; } = DateTime.Now;
        public Guid? CyPCBOrderID { get; set; }
        public Guid? CyUserID { get; set; }
        public Guid? FileID { get; set; }
        public DateTime? SendDate { get; set; }
        public PCBFileType Type { get; set; }
        public string? userName { get; set; }
    }

    public class PCBOrderItemApprovalDTO 
    {
        public virtual Guid ID { get; set; }

        public Guid CyPCBOrderID { get; set; }

        public Guid? CyUserID { get; set; }

        public string? UserName { get; set; }

        public UserType Role { get; set; }

        public string? RoleName { get; set; }

        public bool? IsApproved { get; set; } = false;
        public DateTime? ActionTime { get; set; }
        public string? Comment { get; set; }

        public bool? IsFirst { get; set; } = false;
    }
}
