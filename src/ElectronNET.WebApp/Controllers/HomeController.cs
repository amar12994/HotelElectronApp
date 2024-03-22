using ElectronNET.WebApp.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ElectronNET.WebApp.Controllers
{
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            
            return View();
        }
    }
}
