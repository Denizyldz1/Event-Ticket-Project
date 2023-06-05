using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace TicketProject.WebUI.ViewComponents.Default
{
    public class _PanelUserInfo:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _PanelUserInfo(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            if (User.Identity.IsAuthenticated)
            {

                var claimsPrincipal = User as ClaimsPrincipal;
                var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);


                var client = _httpClientFactory.CreateClient();


                var response = await client.GetAsync($"https://localhost:7259/api/TicketUser/{userId}");
                if (response.IsSuccessStatusCode)
                {

                    var userJson = await response.Content.ReadAsStringAsync();

                    JObject userObject = JObject.Parse(userJson);
                    string name = userObject["name"].ToString();
                    string surname = userObject["surname"].ToString();
                    string id = userObject["id"].ToString();

                    ViewBag.UserInformation = $"{name} {surname} {id}";

                    return View();

                }

            }

            return View();
        }
    }
}
