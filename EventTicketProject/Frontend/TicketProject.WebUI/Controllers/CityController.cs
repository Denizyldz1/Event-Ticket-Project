using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using TicketProject.WebUI.Models.City;
using TicketProject.WebUI.Models.City;

namespace TicketProject.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CityController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CityController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7259/api/City");
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<CityViewModel>>(jsonData);
                return View(values);
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddCity()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCity(AddCityViewModel addCityViewModel)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(addCityViewModel);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8,"application/json");
            var responseMessage = await client.PostAsync("https://localhost:7259/api/City" +
                "", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();

                if (TryParseJson(errorResponse, out var errorObject))
                {
                    var errorMessage = errorObject["errors"]["CityName"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        ModelState.AddModelError("CityName", errorMessage);
                        return View(addCityViewModel);
                    }
                }

                // Gelen veri JSON formatında değilse veya hata mesajı alınamazsa, hatayı direkt olarak modele ekle
                ModelState.AddModelError("CityName", errorResponse);
                return View(addCityViewModel);
            }
        }
        public async Task<IActionResult> DeleteCity(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7259/api/City/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        // Update işlemi yaparken güncelenecek verinin input'da gösterilmesi 
        [HttpGet]
        public async Task<IActionResult> UpdateCity(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7259/api/City/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateCityViewModel>(jsonData);
                return View(values);
            }
            return View();
        }
        // Güncelleme işlemi
        [HttpPost]
        public async Task<IActionResult> UpdateCity(UpdateCityViewModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7259/api/City", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();

                if (TryParseJson(errorResponse, out var errorObject))
                {
                    var errorMessage = errorObject["errors"]["CityName"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        ModelState.AddModelError("CityName", errorMessage);
                        return View(model);
                    }
                }

                // Gelen veri JSON formatında değilse veya hata mesajı alınamazsa, hatayı direkt olarak modele ekle
                ModelState.AddModelError("CityName", errorResponse);
                return View(model);
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
