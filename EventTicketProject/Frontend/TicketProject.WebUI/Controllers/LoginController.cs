using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text;
using TicketProject.DataLayer.Concrete;
using TicketProject.WebUI.Models.Category;
using TicketProject.WebUI.Models.Event;
using TicketProject.WebUI.Models.LoginUser;

namespace TicketProject.WebUI.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<TicketUser> _userManager;
        private readonly SignInManager<TicketUser> _signInManager;


        public LoginController(IHttpClientFactory httpClientFactory, UserManager<TicketUser> userManager, SignInManager<TicketUser> signInManager)
        {
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginUserViewModel loginUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            TicketUser findeduser = await _userManager.FindByEmailAsync(loginUserViewModel.Email);
            if (findeduser == null)
            {
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(findeduser, loginUserViewModel.Password, loginUserViewModel.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Default");
            }
            return View();

        }
        public async Task<IActionResult> LogOut()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7259/api/TicketUser/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ProfileUserModel>(jsonData);
                return View(values);
            }
            return RedirectToAction("Index", "Default");

        }
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileUserModel profileUserViewModel)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(profileUserViewModel);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7259/api/TicketUser", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();

                if (TryParseJson(errorResponse, out var errorObject))
                {
                    var errorMessage = errorObject["errors"]["OldPassword"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        profileUserViewModel.OldPassword = "";
                        ModelState.AddModelError("Title", errorMessage);
                       
                    }
                    var errorPasMessage = errorObject["errors"]["Password"]?[0]?.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        profileUserViewModel.OldPassword = "";
                        ModelState.AddModelError("Title", errorPasMessage);

                    }
                    await Profile(profileUserViewModel.Id);
                    return View(profileUserViewModel);
                }

                // Gelen veri JSON formatında değilse veya hata mesajı alınamazsa, hatayı direkt olarak modele ekle
                ModelState.AddModelError("Password", errorResponse);
                await Profile(profileUserViewModel.Id);
                return View(profileUserViewModel);
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
		public async Task<IActionResult> TicketCancel(int id)
		{
            var claimsPrincipal = User as ClaimsPrincipal;
            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);


            var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.PutAsync($"https://localhost:7259/api/Ticket/TicketCancel/{id}", null);
			if (responseMessage.IsSuccessStatusCode)
			{
              
                await Profile(id);
                return RedirectToAction("Profile", "Login", new { id = userId });
                

            }
            await Profile(id);
            return RedirectToAction("Profile", "Login", new { id = userId });
            

        }
        public async Task<IActionResult> EventCancel(int id)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.PutAsync($"https://localhost:7259/api/Event/EventCancel/{id}", null);
            if (responseMessage.IsSuccessStatusCode)
            {
                await Profile(id);
                return RedirectToAction("Profile", "Login", new { id = userId });

            }
            await Profile(id);
            return RedirectToAction("Profile", "Login", new { id = userId });
        }

	}
}
