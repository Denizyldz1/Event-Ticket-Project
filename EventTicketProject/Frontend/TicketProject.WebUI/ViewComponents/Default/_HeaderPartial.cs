using Microsoft.AspNetCore.Mvc;

namespace TicketProject.WebUI.ViewComponents.Default
{
    public class _HeaderPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
