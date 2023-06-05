namespace TicketProject.WebUI.Models.Ticket
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public string? NameSurname { get; set; }
        public string? TicketNumber { get; set; }
        public bool Status { get; set; }
    }
}
