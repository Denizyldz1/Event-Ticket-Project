using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using TicketProject.WebUI.Models.Event;
using TicketProject.WebUI.Models.Ticket;

namespace TicketProject.WebUI.ViewComponents.Default
{
    public class _TicketPartial:ViewComponent
    {

            private readonly IHttpClientFactory _httpClientFactory;

            public _TicketPartial(IHttpClientFactory httpClientFactory)
            {
                _httpClientFactory = httpClientFactory;
            }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"https://localhost:7259/api/Ticket/(User/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<TicketViewModel>>(jsonData);
                return View(values);
            }
            else
            {
                ViewBag.ErrorMessage = "Aktif Bilet Yok";
                return View();
            }
            
        }



    }
}
