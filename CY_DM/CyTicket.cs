using CY_BM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_DM
{
    public class CyTicket : BaseDM
    {
        public string? Topic { get; set; }
        public string? Title { get; set; }
        //public string? 
        public Guid? UserId { get; set; }
        public virtual CyUser? User { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<CyOrderMessage>? Messages { get; set; }
        public TicketStatus Status { get; set; }

        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}