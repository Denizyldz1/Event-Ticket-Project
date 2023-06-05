using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProject.DtoLayer.Dtos.CityDto
{
    public class AddCityDto
    {
        [Required(ErrorMessage = "Lütfen Bir Şehir Giriniz!")]
        public string? CityName { get; set; }
    }
}
