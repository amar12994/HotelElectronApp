using System;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.WebApp.Domain;
using ElectronNET.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ElectronNET.WebApp.Controllers
{
    public class Order : Controller
    {
        public IActionResult OrderList()
        {
            Electron.IpcMain.RemoveAllListeners("confirm-order");
            _ = Electron.IpcMain.On("confirm-order", async (args) =>
            {
                string cusomerOrderString = await Task.FromResult(args.ToString());
                CustomerOrder customerOrder = JsonConvert.DeserializeObject<CustomerOrder>(cusomerOrderString);
                customerOrder.OrderId = Guid.NewGuid();
                if (customerOrder.IsPayment && customerOrder.IsConfirmed)
                {
                    customerOrder.PaymentDate = DateTime.Now.Date;
                }
                else if (!customerOrder.IsPayment && customerOrder.IsConfirmed)
                {
                    customerOrder.ConfirmedDate = DateTime.Now.Date;
                }

                if (ElectronD.Instance.CustomerOrders == null)
                {
                    ElectronD.Instance.CustomerOrders = new();
                }
                ElectronD.Instance.CustomerOrders.Add(customerOrder);

                await FunctionHelpers.WriteCustomerOrderToFileAsync(ElectronD.Instance.CustomerOrders);

                ElectronD.Instance.CartMenuItems = null;
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "confirm-order-success", customerOrder.OrderId);
            });

            Electron.IpcMain.RemoveAllListeners("complete-order");
            _ = Electron.IpcMain.On("complete-order", async (args) =>
            {
                string orderId = args.ToString();
                var customerOrder = ElectronD.Instance.CustomerOrders.FirstOrDefault(order => order.OrderId.ToString() == orderId);
                if (customerOrder != null)
                {
                    customerOrder.PaymentDate = DateTime.Now.Date;
                    customerOrder.IsPayment = true;
                }
                await FunctionHelpers.WriteCustomerOrderToFileAsync(ElectronD.Instance.CustomerOrders);

                ElectronD.Instance.CartMenuItems = null;
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "complete-order-success", orderId);
            });

            return View();
        }
    }
}
