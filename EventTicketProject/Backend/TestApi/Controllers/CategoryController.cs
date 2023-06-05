using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketProject.BusinessLayer.Category.Business;
using TicketProject.DataLayer.Concrete;
using TicketProject.DtoLayer.Dtos.CategoryDto;

namespace TicketProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;


        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CategoryList()
        {
            var values = _categoryService.TGetAll();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult AddCategory(AddCategoryDto addCategoryDto)
        {
                     
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (IsCategoryNameExists(addCategoryDto.CategoryName))
            {
                return BadRequest("Bu kategori adı zaten mevcut");
            }

            var values = _mapper.Map<Category>(addCategoryDto);
            _categoryService.TInsert(values);
            return Ok();

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var values = _categoryService.TGetById(id);
            _categoryService.TDelete(values);
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = _categoryService.TGetById(updateCategoryDto.CategoryId);

            if (existingCategory == null)
            {
                return NotFound("Güncellenecek kategori bulunamadı");
            }

            if (IsCategoryNameExists(updateCategoryDto.CategoryName, existingCategory.CategoryId))
            {
                return BadRequest("Bu kategori adı zaten mevcut");
            }

            existingCategory.CategoryName = updateCategoryDto.CategoryName;
            // Diğer güncelleme işlemleri

            _categoryService.TUpdate(existingCategory);
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var values = _categoryService.TGetById(id);
            return Ok(values);
        }
        private bool IsCategoryNameExists(string categoryName)
        {
            return _categoryService.TGetAll().Any(c => c.CategoryName.ToLower() == categoryName.ToLower());
        }
        private bool IsCategoryNameExists(string categoryName, int categoryId)
        {
            return _categoryService.TGetAll().Any(c => c.CategoryName.ToLower() == categoryName.ToLower() && c.CategoryId != categoryId);
        }

    }
}
