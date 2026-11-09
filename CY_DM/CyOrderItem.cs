using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CY_BM;
using System.Text.Json.Serialization;

namespace CY_DM
{
    public class CyOrderItem : BaseDM
    {
        public string? PartNumber { get; set; }
        public string? Manufacturer { get; set; }

        
        public Guid? CyProductID { get; set; }
        public virtual CyProduct? CyProduct { get; set; }
      
        
        public Guid? CyOrderID { get; set; }
        [JsonIgnore]
        public virtual CyOrder? CyOrder { get; set; }

        
        
        [Required]
        public int Quantity { get; set; }

        //[Column(TypeName = "int(18,2)")]
        public double? UnitPrice { get; set; }

       // [Column(TypeName = "int(18,2)")]
        public double? TotalPrice { get; set; }
        public OrderItemStatus Status { get; set; }
        public string? StatusText { get; set; }
        public string? Information {  get; set; }
        public string? Duration { get; set; }

        /// <summary>
        /// سمت فروشگاه 
        /// </summary>
        public string? CreatorCommentA { get; set; }


        /// <summary>
        /// سمت انبار
        /// </summary>
        public string? CommentB { get; set; }

        /// <summary>
        /// ایجاد کننده ایتم
        /// </summary>
        /// 
        public Guid? CreatorId { get; set; }
        public string? CreatorName { get; set; }

        public UserType? LevelOk { get; set; }
        public UserType? LevelBOk { get; set; }


        /// <summary>
        /// وضعیت تامین
        /// </summary>
        public string? InSupplyStatus { get; set; }


        public virtual ICollection<OrderItemApproval> Approvals { get; set; } = new List<OrderItemApproval>();

    }



    public class OrderItemApproval : BaseDM
    {
        public Guid CyOrderItemID { get; set; }
        [JsonIgnore]
        public virtual CyOrderItem? CyOrderItem { get; set; }

        public Guid? CyUserID { get; set; }
        public virtual CyUser? CyUser { get; set; }

        public string? UserName { get; set; }

        public UserType Role { get; set; }

        public string? RoleName { get; set; }

        public bool? IsApproved { get; set; } =false;
        public DateTime? ActionTime { get; set; }
        public string? Comment { get; set; }

        public bool? IsFirst { get; set; } = false;
    }
}
