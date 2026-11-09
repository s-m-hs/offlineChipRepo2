using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CY_BM;

namespace CY_DM
{
    public class CyOrderMessage : BaseDM
    {
        //public string Title { get; set; }

        public Guid? SenderID    { get; set; }
        public virtual CyUser? Sender { get; set; }

        public OrderType OrderType { get; set; }
        public Guid? OrderID { get; set; }
       // public virtual CyOrder? Order { get; set; }

        //public Guid? TicketID { get; set; }
        //public virtual CyTicket? Ticket { get; set; }

        public string? Message   { get; set; }
        //  public OrderMessageStatus? Status { get; set; }
        public OrderMessageType? MessageType { get; set; }
        public DateTime? SentDate { get; set; }
        //public DateTime? SeenDate { get; set; }
        public List<Guid> Mentions { get; set; } = new List<Guid>();
        public Guid? FileID { get; set; }
    }
}
