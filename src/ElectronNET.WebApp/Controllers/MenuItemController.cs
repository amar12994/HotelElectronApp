using ElectronNET.WebApp.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ElectronNET.WebApp.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult MenuItemList()
        {
            //var count = _unitOfWork.MenuItemRepository.GetAll();
            //Console.WriteLine($"Count:-{count}");
            return View();
        }
    }
}
