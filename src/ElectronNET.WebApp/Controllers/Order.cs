using ElectronNET.API;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ElectronNET.WebApp.Controllers
{
    public class Order : Controller
    {
        public IActionResult OrderHome()
        {

            _ = Electron.IpcMain.On("button1", (args) =>
            {
                //var serializeTargetStructureList = JsonConvert.SerializeObject(targetStructureList);
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "button1-success", args.ToString());
            });
            return View();

        }
    }
}
