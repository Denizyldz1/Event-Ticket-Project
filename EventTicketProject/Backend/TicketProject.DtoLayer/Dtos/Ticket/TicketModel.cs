using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProject.DtoLayer.Dtos.Ticket
{
    public class TicketModel
    {
        public int TicketId { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public string? NameSurname { get; set; }
        public string? TicketNumber { get; set; }
        public bool Status { get; set; }
    }
}
