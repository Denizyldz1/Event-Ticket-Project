using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using TicketProject.WebUI.Models.Category;
using TicketProject.WebUI.Models.City;
using TicketProject.WebUI.Models.Event;
using TicketProject.WebUI.Models.TicketUser;

namespace TicketProject.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EventController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7259/api/Event");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<EventViewModel>>(jsonData);
                return View(values);
            }
            return View();
        }
        #region Ekleme İşlemi
        [HttpGet]
        public async Task<IActionResult> AddEvent()
        {

                var cities = await GetCities();
                var categories = await GetCategories();
                var users = await GetUsers();

            List<SelectListItem> cityList = (from o in cities.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = o.CityName,
                                                 Value = o.CityName.ToString()
                                             }).ToList();
            ViewBag.cityListBag = cityList;

            List<SelectListItem> categoryList = (from o in categories.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = o.CategoryName,
                                                     Value = o.CategoryName.ToString()
                                                 }).ToList();
            ViewBag.categoryListBag = categoryList;

            List<SelectListItem> userList = (from o in users.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = o.Name + " " + o.Surname,
                                                 Value = o.Name + " " + o.Surname.ToString()
                                             }).ToList();
            ViewBag.userListBag = userList;

            return View();


        }
        [HttpPost]
        public async Task<IActionResult> AddEvent(AddEventViewModel addEventViewModel)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(addEventViewModel);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7259/api/Event" +
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
                    #region ModelState Error Kontrolleri
                    var errorMessage = errorObject["errors"]["Title"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        addEventViewModel.Title = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Title", errorMessage);
                       
                    }

                    var descriptionErrorMessage = errorObject["errors"]["Description"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(descriptionErrorMessage))
                    {
                        addEventViewModel.Description = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Description", descriptionErrorMessage);
                       
                    }

                    var dateErrorMessage = errorObject["errors"]["Date"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(dateErrorMessage))
                    {
                        addEventViewModel.Date = DateTime.Now; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Date", dateErrorMessage);
                        
                    }

                    var addressErrorMessage = errorObject["errors"]["Address"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(addressErrorMessage))
                    {
                        addEventViewModel.Address = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Address", addressErrorMessage);
                       
                    }

                    var capacityErrorMessage = errorObject["errors"]["Capacity"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(capacityErrorMessage) && addEventViewModel.Capacity !=0)
                    {
                        addEventViewModel.Capacity = 0; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Capacity", capacityErrorMessage);
                        
                    }
                    await AddEvent();

                    return View(addEventViewModel);
                    #endregion

                }

                // Gelen veri JSON formatında değilse veya hata mesajı alınamazsa, hatayı direkt olarak modele ekle
                ModelState.AddModelError("Date", errorResponse);
                await AddEvent();
                return View(addEventViewModel);
            }
        }
        #endregion
        #region Admin etkinlik Onayı
        public async Task<IActionResult> AdminApprove(int id)
        {

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.PutAsync($"https://localhost:7259/api/Event/AdminApprove/{id}", null);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AdminRefuse(int id)
        {

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.PutAsync($"https://localhost:7259/api/Event/AdminRefuse/{id}", null);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }
        #endregion

        // Etkinlip iptal etme işlemi
        public async Task<IActionResult> EventCancel(int id)
        {

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.PutAsync($"https://localhost:7259/api/Event/EventCancel/{id}", null);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }
        //Silme işlemi
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7259/api/Event/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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
        #region Günceleme işlemi
        [HttpGet]
        public async Task<IActionResult> UpdateEvent(int id)
        {
            await AddEvent();
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7259/api/Event/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateEventViewModel>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEvent(UpdateEventViewModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7259/api/Event", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();

                if (TryParseJson(errorResponse, out var errorObject))
                {
                    #region ModelState Error Kontrolleri
                    var errorMessage = errorObject["errors"]["Title"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        model.Title = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Title", errorMessage);

                    }

                    var descriptionErrorMessage = errorObject["errors"]["Description"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(descriptionErrorMessage))
                    {
                        model.Description = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Description", descriptionErrorMessage);

                    }

                    var dateErrorMessage = errorObject["errors"]["Date"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(dateErrorMessage))
                    {
                        model.Date = DateTime.Now; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Date", dateErrorMessage);

                    }

                    var addressErrorMessage = errorObject["errors"]["Address"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(addressErrorMessage))
                    {
                        model.Address = ""; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Address", addressErrorMessage);

                    }

                    var capacityErrorMessage = errorObject["errors"]["Capacity"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(capacityErrorMessage) && model.Capacity != 0)
                    {
                        model.Capacity = 0; // Alanı temizleyin veya gerekirse mevcut değeri güncelleyin
                        ModelState.AddModelError("Capacity", capacityErrorMessage);

                    }
                    await AddEvent();

                    return View(model);
                    #endregion

                }

                // Gelen veri JSON formatında değilse veya hata mesajı alınamazsa, hatayı direkt olarak modele ekle
                ModelState.AddModelError("Date", errorResponse);
                await AddEvent();

                return View(model);
            }
        }
        #endregion

        #region SelectList için Veri getirme
        private async Task<List<CityViewModel>> GetCities()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7259/api/City");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var cities = JsonConvert.DeserializeObject<List<CityViewModel>>(jsonData);
                return cities;
            }
            return new List<CityViewModel>();
        }

        private async Task<List<CategoryViewModel>> GetCategories()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7259/api/Category");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<CategoryViewModel>>(jsonData);
                return categories;
            }
            return new List<CategoryViewModel>();
        }

        private async Task<List<UserViewModel>> GetUsers()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7259/api/TicketUser");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserViewModel>>(jsonData);
                return users;
            }
            return new List<UserViewModel>();
        }
        #endregion
    }
}
