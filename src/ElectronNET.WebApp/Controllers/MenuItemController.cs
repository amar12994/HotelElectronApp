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
        public IActionResult MenuItemList()
        {
            Electron.IpcMain.RemoveAllListeners("new-category");
            _ = Electron.IpcMain.On("new-category", async (args) =>
            {
                Category category = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = args.ToString(),
                };
                if (ElectronD.Instance.Cetegories == null)
                {
                    ElectronD.Instance.Cetegories = new();
                }
                ElectronD.Instance.Cetegories.Add(category);
                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);


                var serializedata = JsonConvert.SerializeObject(category);
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "new-category-success", serializedata);
            });

            Electron.IpcMain.RemoveAllListeners("add-update-menu-item");
            _ = Electron.IpcMain.On("add-update-menu-item", async (args) =>
            {
                MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(args.ToString());
                var needToUpdate = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.Equals(menuItem.CategoryId));
                if (needToUpdate.MenuItems == null)
                {
                    needToUpdate.MenuItems = new();
                }
                if (menuItem.Id == Guid.Empty)
                {
                    menuItem.Id = Guid.NewGuid();
                    needToUpdate.MenuItems.Add(menuItem);
                }
                else
                {
                    var updateMenuItem = needToUpdate.MenuItems?.FirstOrDefault(item => item.Id.Equals(menuItem.Id));
                    updateMenuItem.Price = menuItem.Price;
                    updateMenuItem.Name = menuItem.Name;
                }

                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var serializedata = JsonConvert.SerializeObject(menuItem);
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "add-update-menu-item-success", serializedata);
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
                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var serializeAddonData = JsonConvert.SerializeObject(addonItem);
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "new-addon-item-success", serializeAddonData);
            });

            Electron.IpcMain.RemoveAllListeners("cart-items");
            _ = Electron.IpcMain.On("cart-items", async (args) =>
            {
                string cartItemstring = await Task.FromResult(args.ToString());
                List<CartMenuItem> cartItems = JsonConvert.DeserializeObject<List<CartMenuItem>>(cartItemstring);
                if (ElectronD.Instance.CartMenuItems == null)
                {
                    ElectronD.Instance.CartMenuItems = new();
                }
                foreach (CartMenuItem cartItem in cartItems)
                {
                    CartMenuItem needToUpdateCart = ElectronD.Instance.CartMenuItems.FirstOrDefault(item => item.Id == cartItem.Id);
                    if (needToUpdateCart == null)
                    {
                        ElectronD.Instance.CartMenuItems.Add(cartItem);
                    }
                    else
                    {
                        needToUpdateCart.Quantity = needToUpdateCart.Quantity + cartItem.Quantity;
                    }
                }
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                mainWindow.LoadURL($"http://localhost:{BridgeSettings.WebPort}/Order/OrderList");
            });

            Electron.IpcMain.RemoveAllListeners("delete-menu-item");
            _ = Electron.IpcMain.On("delete-menu-item", async (args) =>
            {
                MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(args.ToString());

                var category = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.Equals(menuItem.CategoryId));
                var menuitem = category?.MenuItems?.FirstOrDefault(item => item.Id.Equals(menuItem.Id));
                if (menuitem != null)
                {
                    menuitem.IsDeleted = true;
                }

                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var serializedata = JsonConvert.SerializeObject(menuItem);
                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "delete-menu-item-success", serializedata);
            });
            return View();
        }
    }
}
