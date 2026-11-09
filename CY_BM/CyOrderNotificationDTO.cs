using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class CyOrderNotificationDTO
    {
        public virtual Guid ID { get; set; }


        public virtual DateTime CreateDate { get; set; }

        public Guid? ReceiverID { get; set; }

        public OrderType OrderType { get; set; }


        public Guid? OrderID { get; set; }

        public string? Title { get; set; }
        public NotificationStatus? Status { get; set; }
        public DateTime? SentDate { get; set; }
    }
}
