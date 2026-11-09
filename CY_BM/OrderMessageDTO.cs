using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class OrderMessageDTO
    {
        public Guid ID { get; set; }
        //public string Title { get; set; }

        public Guid? SenderID { get; set; }
        public string? SenderName { get; set; }
        public OrderType OrderType { get; set; }
        public Guid? OrderID { get; set; }

        public string? Message { get; set; }
        public OrderMessageType? MessageType { get; set; }
        public DateTime? SentDate { get; set; }
        //public DateTime? SeenDate { get; set; }
        public List<Guid> Mentions { get; set; } = new List<Guid>();
        public Guid? FileID { get; set; }
    }
    public class OrderMessageBDTO
    {
        public Guid ID { get; set; }
        //public string Title { get; set; }

        public Guid? SenderID { get; set; }
        public string? SenderName { get; set; }
        public OrderType OrderType { get; set; }
        public Guid? OrderID { get; set; }

        public string? Message { get; set; }
        public OrderMessageType? MessageType { get; set; }
        public DateTime? SentDate { get; set; }
        //public DateTime? SeenDate { get; set; }
        public List<string>? Mentions { get; set; } = new();
        public Guid? FileID { get; set; }
    }

    //public class GetMessagesReturnDTO
    //{
    //    public OrderStatus? OrderStatusStatusEnum { get; set; }
    //    public string? OrderStatus { get; set; }
    //    public string? Address { get; set; }
    //    public double? TotalAmount { get; set; }
    //    public Guid? File { get; set; }
    //    public DateTime? OrderDate { get; set; }
    //    public ICollection<OrderMessageDTO>? Messages { get; set; }
    //}

    //public class TicketMessageDTO
    //{
    //    //public string Title { get; set; }
    //    public Guid ID { get; set; }
    //    public Guid? SenderID { get; set; }
    //    public string? SenderName { get; set; }

    //    public Guid? TicketID { get; set; }

    //    public string? Description { get; set; }
    //    public OrderMessageStatus? Status { get; set; }
    //    public DateTime? SentDate { get; set; }
    //    public DateTime? SeenDate { get; set; }

    //    public Guid? FileID { get; set; }
    //}
}
