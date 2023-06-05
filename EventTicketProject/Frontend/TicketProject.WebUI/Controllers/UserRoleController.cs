using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TicketProject.WebUI.Models.Category;
using TicketProject.WebUI.Models.TicketUserRole;

namespace TicketProject.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserRoleController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7259/api/TicketUserRole/GetUsersWithRole");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<UserRoleViewModel>>(jsonData);
                return View(values);
            }
            else
            {
               ViewBag.ErrorMessage = "Kayıt bulunamadı";
     
            }
            return View();

        }
        [HttpGet]
        public IActionResult AddUserRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUserRole(AddRoleModel addRoleModel) 
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(addRoleModel);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync($"https://localhost:7259/api/TicketUserRole?userId={addRoleModel.UserId}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();
                ModelState.AddModelError("UserId", errorResponse);
                return View(addRoleModel);
            }
        }

        public async Task<IActionResult> GetToRole(int Id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7259/api/TicketUserRole?userId={Id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
        
    }
}
