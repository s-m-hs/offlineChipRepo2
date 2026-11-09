using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class TicketDTO
    {
        public Guid? ID {  get; set; }
        public string? Topic { get; set; }
        public string? Title { get; set; }
        public Guid? UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public TicketStatus Status { get; set; }

        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }

}
