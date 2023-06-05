using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProject.DataLayer.Concrete
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int EventID { get; set; }
        public int UserID { get; set; }
        public string? TicketNumber { get; set; }
        public bool Status { get; set; }

    }
}
