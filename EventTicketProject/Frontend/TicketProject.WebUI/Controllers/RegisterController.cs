using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using TicketProject.DataLayer.Concrete;
using TicketProject.WebUI.Models.Event;
using TicketProject.WebUI.Models.TicketUser;

namespace TicketProject.WebUI.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public RegisterController(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(AddUserViewModel addUserViewModel)
        {
            addUserViewModel.UserName = addUserViewModel.Email;
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(addUserViewModel);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("https://localhost:7259/api/TicketUser", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();
                if (TryParseJson(errorResponse, out var errorObject))
                {
                    var errorMessage = errorObject["errors"]["Name"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        addUserViewModel.Name = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Name", errorMessage);

                    }

                    var surNameErrorMessage = errorObject["errors"]["Surname"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(surNameErrorMessage))
                    {
                        addUserViewModel.Surname = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Description", surNameErrorMessage);

                    }

                    var emailErrorMessage = errorObject["errors"]["Email"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(emailErrorMessage))
                    {
                        addUserViewModel.Email = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Date", emailErrorMessage);

                    }

                    var addressErrorMessage = errorObject["errors"]["Address"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(emailErrorMessage))
                    {
                        addUserViewModel.Address = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Address", addressErrorMessage);

                    }

                    var paswordErrorMessage = errorObject["errors"]["Password"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(paswordErrorMessage))
                    {
                        addUserViewModel.Password = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Password", paswordErrorMessage);

                    }


                    return View(addUserViewModel);


                }
                ModelState.AddModelError("Name", errorResponse);
                return View(addUserViewModel);

            }

        }
        private bool TryParseJson(string input, out JObject result)
        {
            result = null;

            try
            {
                result = JsonConvert.DeserializeObject<JObject>(input);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }
    }
}
