using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProject.DtoLayer.Dtos.EventDto
{
    public class AddEventModel
    {
        [Required(ErrorMessage = "Başlık kısmı boş geçilemez")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Açıklama kısmı boş geçilemez")]
        public string? Description { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Adres kısmı boş geçilemez")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Kapasite kısmı boş geçilemez")]
        [Range(0, 9999, ErrorMessage = "Kapasite 0 ile 9999 arasında olmalıdır.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Kapasite sadece rakamlardan oluşmalıdır.")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Şehir Kısmı boş geçilemez")]
        public string? CityName { get; set; }
        [Required(ErrorMessage = "Kategori Kısmı boş geçilemez")]
        public string? CategoryName { get; set; }
        [Required(ErrorMessage = "Kullanıcı kısmı boş geçilemez")]
        public string? FullName { get; set; }
    }
}
