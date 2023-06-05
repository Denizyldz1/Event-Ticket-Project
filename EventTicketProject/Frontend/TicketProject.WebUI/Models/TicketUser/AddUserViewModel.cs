using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TicketProject.WebUI.Models.TicketUser
{
    public class AddUserViewModel
    {

        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
    }
}
