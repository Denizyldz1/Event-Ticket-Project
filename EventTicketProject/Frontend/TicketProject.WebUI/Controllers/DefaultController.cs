using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text;
using TicketProject.DataLayer.Concrete;
using TicketProject.WebUI.Models.Category;
using TicketProject.WebUI.Models.City;
using TicketProject.WebUI.Models.Event;
using TicketProject.WebUI.Models.LoginUser;
using TicketProject.WebUI.Models.TicketUser;

namespace TicketProject.WebUI.Controllers
{

    public class DefaultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<TicketUser> _userManager;

        public DefaultController(IHttpClientFactory httpClientFactory,UserManager<TicketUser> userManager)
        {
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
        }
        public IActionResult Unauthorized()
        {

            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7259/api/Event");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<EventViewModel>>(jsonData);
                if (values.Count == 0)
                {
                    ViewBag.ErrorMessage = "Aktif Etkinlik Yok";
                    return View();
                }
                return View(values);
            }
            else
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = errorResponse;
                return View();
            }
                
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddEvent()
        {

            var cities = await GetCities();
            var categories = await GetCategories();


            List<SelectListItem> cityList = (from o in cities.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = o.CityName,
                                                 Value = o.CityName
                                             }).ToList();
            ViewBag.cityListBag = cityList;

            List<SelectListItem> categoryList = (from o in categories.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = o.CategoryName,
                                                     Value = o.CategoryName
                                                 }).ToList();
            ViewBag.categoryListBag = categoryList;

            return View();


        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddEvent(AddEventViewModel addEventViewModel)
        {
            var userInfo = await _userManager.GetUserAsync(HttpContext.User);
            addEventViewModel.FullName = userInfo.Name + " " + userInfo.Surname;
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
                    if (!string.IsNullOrEmpty(capacityErrorMessage) && addEventViewModel.Capacity != 0)
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
            var claimsPrincipal = User as ClaimsPrincipal;
            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var clientUse = _httpClientFactory.CreateClient();
            var resMessage = await clientUse.GetAsync($"https://localhost:7259/api/TicketUser/{userId}");
            if(resMessage.IsSuccessStatusCode)
            {
                var jsData = await resMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UserViewModel>(jsData);
                model.FullName = values.Name + " " + values.Surname;
            }

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7259/api/Event", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Profile", "Login", new { id = userId });
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


    }
}
