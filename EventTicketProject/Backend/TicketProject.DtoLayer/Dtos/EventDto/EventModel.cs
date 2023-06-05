using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProject.DtoLayer.Dtos.EventDto
{
    public class EventModel
    {
        public int EventId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Address { get; set; }
        public int Capacity { get; set; }
        public string? CityName { get; set; }
        public string? CategoryName { get; set; }
        public string? FullName { get; set; }
        public string? AdminConfirm { get; set; }
        public string? Status { get; set; }
    }
}
