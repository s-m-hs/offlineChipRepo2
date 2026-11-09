using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CY_BM;

namespace CY_DM
{
    public class CyOrderNotification : BaseDM
    {
        //public string Title { get; set; }

        public Guid? ReceiverID { get; set; }
        public virtual CyUser? Receiver { get; set; }

        public OrderType OrderType { get; set; }


        public Guid? OrderID { get; set; }
        //public virtual CyOrder? Order { get; set; }

        //public Guid? TicketID { get; set; }
        //public virtual CyTicket? Ticket { get; set; }

        public string? Title { get; set; }
        public NotificationStatus? Status { get; set; }
        //  public OrderMessageType? MessageType { get; set; }
        public DateTime? SentDate { get; set; }
        //public DateTime? SeenDate { get; set; }
        //  public List<Guid> Mentions { get; set; }
        // public Guid? FileID { get; set; }
    }
}
