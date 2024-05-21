using ElectronNET.API;
using ElectronNET.WebApp.Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace ElectronNET.WebApp.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            Electron.IpcMain.RemoveAllListeners("get-simple-bar-graph-data");
            _ = Electron.IpcMain.On("get-simple-bar-graph-data", (args) =>
            {
                var customerOrders = ElectronD.Instance.CustomerOrders.Where(x => x.IsPayment).GroupBy(g => g.PaymentDate.Value.Date).ToList();
                var orders = (from order in customerOrders
                              select new
                              {
                                  key = order.Key.Date.ToString("d MMM"),
                                  value = order.Count()
                              }).ToArray();

                var serializedata = JsonConvert.SerializeObject(orders);
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "get-simple-bar-graph-data-success", serializedata);
            });
            return View();
        }
    }
}