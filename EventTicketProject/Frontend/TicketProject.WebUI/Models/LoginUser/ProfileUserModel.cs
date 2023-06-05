using System.ComponentModel.DataAnnotations;

namespace TicketProject.WebUI.Models.LoginUser
{
    public class ProfileUserModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? OldPassword { get; set; }

        public string? Password { get; set; }

        public string? PasswordConfirm { get; set; }
    }
}
