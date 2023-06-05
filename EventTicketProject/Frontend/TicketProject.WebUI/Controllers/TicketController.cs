using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using TicketProject.WebUI.Models.Category;
using TicketProject.WebUI.Models.Ticket;

namespace TicketProject.WebUI.Controllers
{
 
    public class TicketController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TicketController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7259/api/Ticket");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<TicketViewModel>>(jsonData);
                return View(values);
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> AddTicket(int id, AddTicketViewModel addTicketViewModel)
        {
            addTicketViewModel.EventId = id;

			var claimsPrincipal = User as ClaimsPrincipal;
			var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
			var newUserId = Int32.Parse(userId);
			addTicketViewModel.UserId = newUserId;

			var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(addTicketViewModel);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7259/api/Ticket", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("TicketOnayMessage", "Ticket");
            }
            else
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = errorResponse;
                return RedirectToAction("TicketRedMessage", "Ticket");

            }

        }
        // Bilet Kontrol İşlemleri
        [HttpGet]
        [Authorize(Roles = "Yetkili, Admin")]

        public IActionResult TicketControl()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Yetkili, Admin")]

        public async Task<IActionResult> TicketControl(string ticketNumber)
        {

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7259/api/Ticket/ticketNumber?ticketNumber={ticketNumber}");

            if (responseMessage.IsSuccessStatusCode)
            {

                return RedirectToAction("TicketControlResult", "Ticket" ,new {ticketNumber});

            }
            else
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();
                ModelState.AddModelError("TicketNumber", errorResponse);
                return View();
            }
        }


        [HttpGet]
        [Authorize(Roles = "Yetkili, Admin")]

        public async Task<IActionResult> TicketControlResult(string ticketNumber)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7259/api/Ticket/ticketNumber?ticketNumber={ticketNumber}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<TicketViewModel>(jsonData);
                return View(values);
            }
            return View();
        }

        public async Task<IActionResult> GetUserTicket(int UserId)
        {

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7259/api/Ticket/(User/{UserId}");

            if (responseMessage.IsSuccessStatusCode)
            {
                return View();

            }

            return View();
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> TicketCancel(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.PutAsync($"https://localhost:7259/api/Ticket/TicketCancel/{id}", null);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }
        public IActionResult TicketOnayMessage()
        {
           
            return View();
        }
        public IActionResult TicketRedMessage()
        {
            var errorMessage = TempData["ErrorMessage"];
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }


    }
}
