using System.Linq;
using ElectronNET.API;
using Microsoft.AspNetCore.Mvc;

namespace ElectronNET.WebApp.Controllers
{
    public class Order : Controller
    {
        public IActionResult OrderList()
        {
            Electron.IpcMain.RemoveAllListeners("order-list");
            _ = Electron.IpcMain.On("order-list", (args) =>
            {
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "order-list-success", "Test");
            });
            return View();
        }
    }
}
