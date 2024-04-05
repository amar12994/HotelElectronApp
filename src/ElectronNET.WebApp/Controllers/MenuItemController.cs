using ElectronNET.API;
using ElectronNET.WebApp.Domain;
using ElectronNET.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronNET.WebApp.Controllers
{
    public class MenuItemController : Controller
    {
        public MenuItemController()
        {
            ElectronD.Instance.Cetegories = FunctionHelpers.JsonFileReadAsync<List<Category>>();
        }

        public IActionResult MenuItemList()
        {
            Electron.IpcMain.RemoveAllListeners("category-list");
            _ = Electron.IpcMain.On("category-list", (args) =>
            {
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "category-list-success", "Test");
            });

            Electron.IpcMain.RemoveAllListeners("new-category");
            _ = Electron.IpcMain.On("new-category", async (args) =>
            {
                Category category = new()
                {
                    Id = Guid.NewGuid(),
                    Name = args.ToString()
                };
                ElectronD.Instance.Cetegories.Add(category);
                await FunctionHelpers.WriteToFileAsync(ElectronD.Instance.Cetegories);


                var serializedata = JsonConvert.SerializeObject(category);
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "new-category-success", serializedata);
            });

            Electron.IpcMain.RemoveAllListeners("new-menu-item");
            _ = Electron.IpcMain.On("new-menu-item", async (args) =>
            {
                MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(args.ToString());
                menuItem.Id = Guid.NewGuid();

                var needToUpdate = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.Equals(menuItem.CategoryId));
                if (needToUpdate.MenuItems == null)
                {
                    needToUpdate.MenuItems = new();
                }
                needToUpdate.MenuItems.Add(menuItem);

                await FunctionHelpers.WriteToFileAsync(ElectronD.Instance.Cetegories);

                var serializedata = JsonConvert.SerializeObject(menuItem);
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "new-menu-item-success", serializedata);
            });

            Electron.IpcMain.RemoveAllListeners("get-menu-basedon-category");
            _ = Electron.IpcMain.On("get-menu-basedon-category", async (args) =>
            {
                string categoryId = await Task.FromResult(args.ToString());
                var menuItems = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.ToString() == categoryId)?.MenuItems;
                var serializedata = JsonConvert.SerializeObject(menuItems);
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "get-menu-basedon-category-success", serializedata);
            });

            Electron.IpcMain.RemoveAllListeners("new-addon-item");
            _ = Electron.IpcMain.On("new-addon-item", async (args) =>
            {
                string addonItemstring = await Task.FromResult(args.ToString());
                AddonItem addonItem = JsonConvert.DeserializeObject<AddonItem>(addonItemstring);
                addonItem.Id = Guid.NewGuid();

                var menuItems = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id == addonItem.CategoryId)?.MenuItems;
                var needToUpdateAddonItems = menuItems?.FirstOrDefault(item => item.Id == addonItem.MenuItemId);
                if (needToUpdateAddonItems?.AddonItems == null)
                {
                    needToUpdateAddonItems.AddonItems = new();
                }
                needToUpdateAddonItems.AddonItems.Add(addonItem);
                await FunctionHelpers.WriteToFileAsync(ElectronD.Instance.Cetegories);

                var serializeAddonData = JsonConvert.SerializeObject(addonItem);
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "new-addon-item-success", serializeAddonData);
            });

            //Electron.IpcMain.RemoveAllListeners("add-cart");
            //_ = Electron.IpcMain.On("add-cart", async (args) =>
            //{
            //    string cartItemstring = await Task.FromResult(args.ToString());
            //    CartMenuItem cartItem = JsonConvert.DeserializeObject<CartMenuItem>(cartItemstring);
            //    if (ElectronD.Instance.CartMenuItems == null)
            //    {
            //        ElectronD.Instance.CartMenuItems = new();
            //    }
            //    ElectronD.Instance.CartMenuItems.Add(cartItem);
            //    var mainWindow = Electron.WindowManager.BrowserWindows.Last();
            //    Electron.IpcMain.Send(mainWindow, "add-cart-success", cartItem.Id);
            //});
            return View();
        }
    }
}
