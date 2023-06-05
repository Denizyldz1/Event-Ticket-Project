using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketProject.BusinessLayer.Event.Business;
using TicketProject.BusinessLayer.TicketBusiness;
using TicketProject.DataLayer.Concrete;
using TicketProject.DtoLayer.Dtos.Ticket;

namespace TicketProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        [HttpGet]
        public IActionResult TicketList()
        {

            var values = _ticketService.TGetTicketAll();
            return Ok(values);
        }
        // Bilet numarası ile konrol yapan mekanizma
        [HttpGet("ticketNumber")]
        public IActionResult TicketList(string ticketNumber)
        {
            var values = _ticketService.TGetByTicketNumber(ticketNumber);
            if(values.Status == false)
            {
                return BadRequest("Bilet iptal edilmiş geçersiz");
            }
            if(values.TicketNumber == null)
            {
                return BadRequest("Bilet bulunamadı");
            }
           return Ok(values);
        }
        [HttpGet("(User/{userId}")]
        public IActionResult UserTicketList(int userId)
        {
           var values = _ticketService.TGetByUserId(userId);
            if(values.Count > 0)
            {
                return Ok(values);
            }
            return BadRequest("Bilet bulunamadı");
        }
        [HttpPut("TicketCancel/{id}")]
        public IActionResult TicketCancel(int id)
        {
            _ticketService.TicketCancel(id);
            return Ok();
        }
        [HttpPost]
        public IActionResult AddTicket(AddTicketModel addTicketModel)
        {
           var value= _ticketService.TTicketInsert(addTicketModel);
            if(value== "Success")
            {
                return Ok();
            }
            else if(value== "CapacityFull")
            {
                return BadRequest("Kapasite Dolu");
            }
            else
            {
                return BadRequest("Mükerrer bilet");
            }
            

         }

    }
}
