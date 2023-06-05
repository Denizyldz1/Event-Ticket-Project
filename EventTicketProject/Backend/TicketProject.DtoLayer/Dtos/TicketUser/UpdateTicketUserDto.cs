using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProject.DtoLayer.Dtos.TicketUser
{
    public class UpdateTicketUserDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ad alanı gereklidir")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Soyad alanı gereklidir")]
        public string? Surname { get; set; }
        [Required(ErrorMessage = "Adres alanı gereklidir")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Email alanı gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı gereklidir")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$", ErrorMessage = "Şifre en az bir alfanümerik olmayan karakter, bir rakam ve bir büyük harf içermelidir.")]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string? PasswordConfirm { get; set; }
        [Required(ErrorMessage = "Şifre alanı gereklidir")]
        public string? OldPassword { get; set; }
    }
}
