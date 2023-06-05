﻿namespace TicketProject.WebUI.Models.Event
{
    public class UpdateEventViewModel
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
    }
}