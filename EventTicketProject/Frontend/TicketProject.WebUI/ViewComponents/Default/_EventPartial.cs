using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using TicketProject.WebUI.Models.Event;

namespace TicketProject.WebUI.ViewComponents.Default
{
    public class _EventPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _EventPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"https://localhost:7259/api/Event/GetUserId/{userId}");
			var jsonData = await response.Content.ReadAsStringAsync();
			var values = JsonConvert.DeserializeObject<List<EventViewModel>>(jsonData);
			if (values.Count == 0)
			{
				ViewBag.ErrorMessage = "Aktif Etkinlik Yok";
				return View();
			}
			return View(values);
		}

    }
}
