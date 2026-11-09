using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class OrderItemDTO
    {
        public Guid? ID { get; set; }
        public virtual DateTime CreateDate { get; set; }= DateTime.Now;
        public Guid? CyProductID { get; set; }
        public Guid? CyOrderID { get; set; }

        [Required]
        public int Quantity { get; set; }

        //[Column(TypeName = "int(18,2)")]
        public double UnitPrice { get; set; }

        // [Column(TypeName = "int(18,2)")]
        public double TotalPrice { get; set; }
        public string? PartNumber { get; set; }
        public string? Manufacturer { get; set; }
        public string? Duration { get; set; }

        public string? Information { get; set; }

        public string? CreatorCommentA { get; set; }

        public string? CommentB { get; set; }

        public Guid? CreatorId { get; set; }
        public string? CreatorName { get; set; }

        public UserType? LevelOk { get; set; }
        public UserType? LevelBOk { get; set; }
        public string? InSupplyStatus { get; set; }


        public virtual ICollection<OrderItemApprovalDTO>? Approvals { get; set; } 
    }




    public class OrderItemApprovalDTO 
    {
        public Guid? ID { get; set; }

        public virtual DateTime CreateDate { get; set; } = DateTime.Now;

        public virtual DateTime? LastModified { get; set; }

        public string? CreatorName { get; set; }

        public Guid? CyOrderItemID { get; set; }

        public Guid? CyPCBOrderID { get; set; }

        public Guid? CyUserID { get; set; }

        public string? UserName { get; set; }

        public UserType Role { get; set; }

        public string? RoleName { get; set; }
        public bool? IsApproved { get; set; } 
        public DateTime? ActionTime { get; set; } 
        public string? Comment { get; set; }
        public bool? IsFirst { get; set; } = false;



    }









    public class OrderItemReturnDTO
    {
        public Guid ID { get; set; }
        public OrderItemStatus Status { get; set; }
        public string? StatusText { get; set; }
        public string? Information { get; set; }
        public Guid? CyProductID { get; set; }
        public string? CyProductName { get; set; }
        public int Quantity { get; set; }
        public string? CyProductImgUrl { get; set; }
        public Guid? CyOrderID { get; set; }

        //[Required]
        //public int Quantity { get; set; }

        public double? UnitPrice { get; set; }
        public double? UnitOfferPrice { get; set; }
               
        public double? TotalPrice { get; set; }
        public string? PartNumber { get; set; }
        public string? Manufacturer { get; set; }
        public string? Duration { get; set; }
        public string? ProductCode { get; set; }
        public int? Supply { get; set; }

    }

    public class AddOrderItemDTO
    {
        public Guid? CyProductID { get; set; }
        public Guid? OrderItemID { get; set; }

        [Required]
        public int Quantity { get; set; }
    }

    public class OrderItemVadidationDTO
    {
        public Guid ID { get; set; }
        public OrderItemStatus Status { get; set; }
        public int Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public string? Duration { get; set; }
    }

}
