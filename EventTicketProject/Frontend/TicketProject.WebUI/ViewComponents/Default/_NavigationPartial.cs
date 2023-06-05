using Microsoft.AspNetCore.Mvc;

namespace TicketProject.WebUI.ViewComponents.Default
{
    public class _NavigationPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
