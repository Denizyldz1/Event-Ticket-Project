using Microsoft.AspNetCore.Mvc;

namespace TicketProject.WebUI.ViewComponents.Default
{
    public class _ScriptPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
