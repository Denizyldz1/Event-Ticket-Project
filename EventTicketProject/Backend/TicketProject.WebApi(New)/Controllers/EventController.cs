using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketProject.BusinessLayer.Category.Business;
using TicketProject.BusinessLayer.Event.Business;
using TicketProject.DataLayer.Concrete;
using TicketProject.DtoLayer.Dtos.CategoryDto;
using TicketProject.DtoLayer.Dtos.EventDto;

namespace TicketProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [HttpGet]
        public IActionResult EventList() 
        {

           var values = _eventService.TGetEventAll();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult AddEvent(AddEventModel addEventModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (addEventModel.Date <= DateTime.Now)
            {
                return BadRequest("Geçmiş tarihli etkinlik düzenleyemezsiniz");
            }
            _eventService.TInsert(addEventModel);
            return Ok();

        }
        [HttpGet("{id}")]
        public IActionResult GetEvent(int id)
        {
            var values = _eventService.TGetByEventId(id);           
            if(values == null)
            {
                return BadRequest("Aktif etkinlik bulunamadı");
            }
            return Ok(values);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var values = _eventService.TGetById(id);
            _eventService.TDelete(values);
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateEvent(UpdateEventModel updateEventModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (updateEventModel.Date < DateTime.Now)
            {
                return BadRequest("Geçmiş tarihli etkinlik düzenleyemezsiniz");

            }
            if (DateTime.Today.AddDays(5) == updateEventModel.Date.Date)
            {
                return BadRequest("Etkinlik 5 gün kala güncelenemez.");
            }
            _eventService.TUpdate(updateEventModel);
            return Ok();
        }

        [HttpPut("AdminApprove/{id}")]

        public IActionResult AdminApprove(int id)
        {
            var values = _eventService.TGetById(id);
            if(values.Status== "Etkinlik İptal Edildi" && values.Status == "Reddedildi" && values.Status == "SüresiDoldu")
            {
                return BadRequest("Onaylanamaz");
            }
            _eventService.AdminConfirmAprove(id); return Ok();
        }
        [HttpPut("AdminRefuse/{id}")]

        public IActionResult AdminRefuse(int id)
        {
            var values = _eventService.TGetById(id);
            if (values.Status == "Etkinlik İptal Edildi" && values.Status == "Onaylandı" && values.Status == "SüresiDoldu")
            {
                return BadRequest("Reddedilemez");
            }
            _eventService.AdminConfirmRefuse(id); return Ok();
        }
        [HttpPut("EventCancel/{id}")]

        public IActionResult EventCancel(int id)
        {
            var values = _eventService.TGetById(id);
            if (values.Status == "Etkinlik İptal Edildi" && values.Status == "Reddedildi")
            {
                return BadRequest("İptal Edilemez");
            }
            if(DateTime.Today.AddDays(5)==values.Date.Date)
            {
                return BadRequest("Etkinlik 5 gün kala iptal edilemez.");
            }
            _eventService.AdminConfirmRefuse(id); return Ok();
        }
        //Filtrelemeler

        [HttpGet("CategoryName")]

        public IActionResult GetCategoryName(string categoryName)
        {

            var values = _eventService.TGetEventCategoryName(categoryName);
            if(values.Count == 0)
            {
                return BadRequest("Kategoride etkinlik bulunamadı");
            }
            return Ok(values);
        }
        [HttpGet("CityName")]

        public IActionResult GetCityName(string cityName)
        {
            var values = _eventService.TGetEventCityName(cityName);
            if(values.Count == 0)
            {
                return BadRequest("Şehir'de etkinlik bulunamadı");

            }
            return Ok(values);
        }
        [HttpGet("GetUserId/{id}")]

        public IActionResult GetUserId(int id)
        {
            var values = _eventService.TGetUserId(id);
            return Ok(values);
        }
    }
}
