using Microsoft.AspNetCore.Mvc;

namespace TicketProject.WebUI.ViewComponents.Default
{
    public class _FooterPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
