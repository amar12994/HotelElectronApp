using Microsoft.AspNetCore.Mvc;

namespace ElectronNET.WebApp.Controllers
{
    public class Order : Controller
    {
        public IActionResult OrderList()
        {
            return View();
        }
    }
}
