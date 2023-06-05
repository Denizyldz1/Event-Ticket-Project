using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProject.DataLayer.Concrete
{
    public class Event
    {
        public int EventId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Address { get; set; }
        public int Capacity { get; set; }
        public int CityID { get; set; }
        public int CategoryID { get; set; }
        public int UserID { get; set; }
        public string? AdminConfirm { get; set; }
        public string? Status { get; set; }

    }
}
