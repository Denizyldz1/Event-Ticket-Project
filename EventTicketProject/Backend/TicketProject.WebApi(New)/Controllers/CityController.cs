using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketProject.BusinessLayer.Category.Business;
using TicketProject.BusinessLayer.City.Business;
using TicketProject.DataLayer.Concrete;
using TicketProject.DtoLayer.Dtos.CategoryDto;
using TicketProject.DtoLayer.Dtos.CityDto;

namespace TicketProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;

        public CityController(ICityService cityService, IMapper mapper)
        {
            _cityService = cityService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CityList()
        {
            var values = _cityService.TGetAll();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult AddCity(AddCityDto addCityDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (IsCityNameExists(addCityDto.CityName) == true)
            {
                return BadRequest("Bu şehir zaten mevcut");
            }
            var values = _mapper.Map<City>(addCityDto);
            _cityService.TInsert(values);
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCity(int id)
        {
            var values = _cityService.TGetById(id);
            _cityService.TDelete(values);
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateCity(UpdateCityDto updateCityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCity = _cityService.TGetById(updateCityDto.CityId);

            if (existingCity == null)
            {
                return NotFound("Güncellenecek kategori bulunamadı");
            }

            if (IsCategoryNameExists(updateCityDto.CityName, existingCity.CityId))
            {
                return BadRequest("Bu kategori adı zaten mevcut");
            }

            existingCity.CityName = updateCityDto.CityName;
            // Diğer güncelleme işlemleri

            _cityService.TUpdate(existingCity);
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var values = _cityService.TGetById(id);
            return Ok(values);
        }
        private bool IsCityNameExists(string cityName)
        {
            return _cityService.TGetAll().Any(c => c.CityName.ToLower() == cityName.ToLower());
        }
        private bool IsCategoryNameExists(string cityName, int cityId)
        {
            return _cityService.TGetAll().Any(c => c.CityName.ToLower() == cityName.ToLower() && c.CityId != cityId);
        }
    }
}
