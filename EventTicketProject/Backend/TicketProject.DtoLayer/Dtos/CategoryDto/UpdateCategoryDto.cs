﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProject.DtoLayer.Dtos.CategoryDto
{
    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Lütfen Bir Kategori Giriniz!")]
        public string? CategoryName { get; set; }
    }
}
