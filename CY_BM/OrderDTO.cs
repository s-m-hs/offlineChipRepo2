using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
   
    public class OrderDTO
    {
        public Guid? ID { get; set; }
        public Guid? CyUserID { get; set; }
        public string? Name { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public DateTime? InProccessDate { get; set; }
        public DateTime? FinalizedDate { get; set; }
        public DateTime? StartOrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }


        //public Ordermode OrderMode { get; set; }
        public string? StatusText { get; set; }

        public double? TotalAmount { get; set; }


        /// <summary>
        /// شماره یا کد پروژه 
        /// </summary>
        public int? OrderCode { get; set; }

        public Currency? Curency { get; set; }
        /// <summary>
        /// ایجادکننده سفارش
        /// </summary>
        //public Guid? CreatorId { get; set; }
        //public string? Creator { get; set; }

        /// <summary>
        /// موظف به انجام سفارش
        /// </summary>
        //public Guid? DoerId { get; set; }
        //public string? Doer { get; set; }


        public Guid? CyAddressID { get; set; }

        public Guid? CyVahedId { get; set; }

        public Guid? CyGoroohId { get; set; }



        public virtual ICollection<OrderItemDTO>? CyOrderItems { get; set; }
        public Guid? BomFileID { get; set; }
        public bool? Notification {  get; set; }
        public int? RowCount { get; set; }
    }



    public class OrderReturnDTO
    {
        public Guid ID { get; set; }
        public Guid? CyUserID { get; set; }
        public string? UserName { get; set; }
        public Guid? OrderCode { get; set; }
        [Required]
        public DateTime? OrderDate { get; set; }

        public DateTime? InProcessDate { get; set; }
        public DateTime? FinalizedDate { get; set; }

        //[Required]
        //public OrderStatus Status { get; set; }
        public string? StatusText { get; set; }

        public double TotalAmount { get; set; }

        public Guid? CyAddressID { get; set; }
        public ICollection<OrderItemReturnDTO> CyOrderItems { get; set; }
        public bool? Notification {  get; set; }

    }

    //public class PcbOrderReturnDTO
    //{
    //    public Guid ID { get; set; }
    //    public Guid? CyUserID { get; set; }
    //    public string? Name { get; set; }
    //    public int? OrderCode { get; set; }
    //    [Required]
    //    public DateTime OrderDate { get; set; }

    //    //[Required]
    //    //public OrderStatus Status { get; set; }
    //    public string? StatusText { get; set; }
    //    public OrderStatus Status { get; set; }
    //    //public int TotalAmount { get; set; }

    //    public Guid? CyAddressID { get; set; }
    //    public pcbDTO? PcbForm { get; set; }
    //    public bool? Notification { get; set; }
    //    //public ICollection<OrderItemReturnDTO> CyOrderItems { get; set; }
    //}

    //public class InspectionOrderReturnDTO
    //{
    //    public Guid ID { get; set; }
    //    public Guid? CyUserID { get; set; }
    //    public string? Name { get; set;}
    //    public int? OrderCode { get; set; }
    //    [Required]
    //    public DateTime OrderDate { get; set; }

    //    //[Required]
    //    //public OrderStatus Status { get; set; }
    //    public string? StatusText { get; set; }
    //    public OrderStatus Status { get; set; }

    //    //public int TotalAmount { get; set; }

    //    public Guid? CyAddressID { get; set; }
    //    public required InspectionFormDTO InspectionForm { get; set; }
    //    public bool? Notification { get; set; }
    //    //public ICollection<OrderItemReturnDTO> CyOrderItems { get; set; }
    //}


    public class OrderByExcelDTO()
    {
        public Guid fileId { get; set; }
        public int addressId { get; set; }
    }



    public class OrderSelect ()
    {
        public Guid? id { get; set; }

        public int? orderCode { get; set; }

        public string? cyVahed { get; set; }
        public string? cyGorooh { get; set; }

        public DateTime? createDate { get; set; }

        public string? cyUser { get; set; }

        public double? totalAmount { get; set; }
        public OrderStatus? orderStatus { get; set; }

        public DateTime? startOrderDate { get; set; }

        public Currency? curency { get; set; }


    }
    public class OrderPcbSelect()
    {
        public Guid? id { get; set; }

        public int? PcbNumber { get; set; }
        public int? OrderCode { get; set; }

        public string? cyVahed { get; set; }
        public string? cyGorooh { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? cyUser { get; set; }
        public string? CreatorName { get; set; }
        public string? Duration { get; set; }

        public double? TotalAmount { get; set; }
        public OrderStatus? Status { get; set; }

    }
    public class ProductSelect()
    {
        public Guid? id { get; set; }

        public string? partNo { get; set; }
        public string? cyProductCategory { get; set; }
        public string? cyManufacturer { get; set; }
        public string? description { get; set; }

        public double? price { get; set; }

        public int? supply { get; set; }

        public string? smallImage { get; set; }

    }
    public class ProductPageSelect()
        {
        public int? TotalCount { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public List<ProductSelect>? Products { get; set; }


    }

}
