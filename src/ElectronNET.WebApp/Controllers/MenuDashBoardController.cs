using ElectronNET.API;
using ElectronNET.WebApp.Domain;
using ElectronNET.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronNET.WebApp.Controllers
{
    public class MenuDashBoardController : Controller
    {
        public IActionResult DashBoardMenu()
        {
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
            return View();
        }

        public IActionResult MenuCategory()
        {
            Electron.IpcMain.RemoveAllListeners("new-category");
            _ = Electron.IpcMain.On("new-category", async (args) =>
            {
                Category category = new()
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

                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "new-category-success", string.Empty);
            });

            Electron.IpcMain.RemoveAllListeners("edit-category");
            _ = Electron.IpcMain.On("edit-category", async (args) =>
            {
                Category cat = JsonConvert.DeserializeObject<Category>(args.ToString());
                var category = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.Equals(cat.Id));
                if (category != null)
                {
                    category.Name = cat.Name;
                }
                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "edit-category-success", string.Empty);
            });


            Electron.IpcMain.RemoveAllListeners("delete-category");
            _ = Electron.IpcMain.On("delete-category", async (args) =>
            {
                string categoryId = args.ToString();
                var category = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.ToString() == categoryId);
                if (category != null)
                {
                    category.IsDeleted = true;
                }
                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "delete-category-success", string.Empty);
            });

            Electron.IpcMain.RemoveAllListeners("reload-category");
            _ = Electron.IpcMain.On("reload-category", (args) =>
            {
                RefreshDataAndRedirect("MenuDashBoard/MenuCategory");
            });

            return View();
        }

        public IActionResult MenuItems()
        {
            Electron.IpcMain.RemoveAllListeners("add-menu-item");
            _ = Electron.IpcMain.On("add-menu-item", async (args) =>
            {
                MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(args.ToString());
                var needToUpdate = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.Equals(menuItem.CategoryId));
                if (needToUpdate.MenuItems == null)
                {
                    needToUpdate.MenuItems = new();
                }
                menuItem.Id = Guid.NewGuid();
                menuItem.Picture = FunctionHelpers.CopyFile(menuItem.Picture);
                needToUpdate.MenuItems.Add(menuItem);

                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "add-menu-item-success", string.Empty);
            });

            Electron.IpcMain.RemoveAllListeners("update-menu-item");
            _ = Electron.IpcMain.On("update-menu-item", async (args) =>
            {
                MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(args.ToString());
                var needToUpdate = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.Equals(menuItem.CategoryId));

                var updateMenuItem = needToUpdate.MenuItems.FirstOrDefault(item => item.Id.Equals(menuItem.Id));
                updateMenuItem.Price = menuItem.Price;
                updateMenuItem.Name = menuItem.Name;
                updateMenuItem.Picture = FunctionHelpers.CopyFile(menuItem.Picture);

                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "update-menu-item-success", string.Empty);
            });

            Electron.IpcMain.RemoveAllListeners("delete-menu-item");
            _ = Electron.IpcMain.On("delete-menu-item", async (args) =>
            {
                MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(args.ToString());
                var needToUpdate = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.Equals(menuItem.CategoryId));

                var updateMenuItem = needToUpdate.MenuItems.FirstOrDefault(item => item.Id.Equals(menuItem.Id));
                updateMenuItem.IsDeleted = true;

                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "delete-menu-item-success", string.Empty);
            });

            Electron.IpcMain.RemoveAllListeners("reload-menu-item");
            _ = Electron.IpcMain.On("reload-menu", (args) =>
            {
                RefreshDataAndRedirect("MenuDashBoard/MenuItems");
            });

            return View();
        }

        public IActionResult MenuAddonItems()
        {
            Electron.IpcMain.RemoveAllListeners("load-menu-basedon-category");
            _ = Electron.IpcMain.On("load-menu-basedon-category", (args) =>
            {
                string categoryId = args.ToString();

                List<MenuItem> menuItems = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.ToString() == categoryId)?.MenuItems?.Where(item => !item.IsDeleted)?.ToList();
                if (menuItems != null && menuItems.Count > 0)
                {
                    // menuItems.MenuItems = new();
                }
                var serializedata = JsonConvert.SerializeObject(menuItems);

                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "load-menu-basedon-category-success", serializedata);
            });

            Electron.IpcMain.RemoveAllListeners("add-addon-item");
            _ = Electron.IpcMain.On("add-addon-item", async (args) =>
            {
                AddonItem addonItem = JsonConvert.DeserializeObject<AddonItem>(args.ToString());
                var needToUpdate = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.Equals(addonItem.CategoryId));
                if (needToUpdate.MenuItems == null)
                {
                    needToUpdate.MenuItems = new();
                }
                var updateMenuItem = needToUpdate.MenuItems.FirstOrDefault(item => item.Id.Equals(addonItem.MenuItemId));
                if (updateMenuItem?.AddonItems == null)
                {
                    updateMenuItem.AddonItems = new();
                }
                addonItem.Id = Guid.NewGuid();
                updateMenuItem.AddonItems.Add(addonItem);

                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "add-addon-item-success", string.Empty);
            });

            Electron.IpcMain.RemoveAllListeners("update-addon-item");
            _ = Electron.IpcMain.On("update-addon-item", async (args) =>
            {
                AddonItem addonItem = JsonConvert.DeserializeObject<AddonItem>(args.ToString());
                var category = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.Equals(addonItem.CategoryId));

                var menuItem = category.MenuItems.FirstOrDefault(item => item.Id.Equals(addonItem.MenuItemId));
                var addonItemNeedToUpdate = menuItem.AddonItems.FirstOrDefault(item => item.Id.Equals(addonItem.Id));
                addonItemNeedToUpdate.Name = addonItem.Name;
                addonItemNeedToUpdate.Price = addonItem.Price;

                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "update-addon-item-success", string.Empty);
            });

            Electron.IpcMain.RemoveAllListeners("delete-addon-item");
            _ = Electron.IpcMain.On("delete-addon-item", async (args) =>
            {
                AddonItem addonItem = JsonConvert.DeserializeObject<AddonItem>(args.ToString());
                var category = ElectronD.Instance.Cetegories.FirstOrDefault(category => category.Id.Equals(addonItem.CategoryId));

                var menuItem = category.MenuItems.FirstOrDefault(item => item.Id.Equals(addonItem.MenuItemId));
                var addonItemNeedToUpdate = menuItem.AddonItems.FirstOrDefault(item => item.Id.Equals(addonItem.Id));
                addonItemNeedToUpdate.IsDeleted = true;

                await FunctionHelpers.WriteCategoryMenuItemsToFileAsync(ElectronD.Instance.Cetegories);

                var mainWindow = Electron.WindowManager.BrowserWindows.Last();
                Electron.IpcMain.Send(mainWindow, "delete-addon-item-success", string.Empty);
            });

            Electron.IpcMain.RemoveAllListeners("reload-addon-item");
            _ = Electron.IpcMain.On("reload-addon-item", (args) =>
            {
                RefreshDataAndRedirect("MenuDashBoard/MenuAddonItems");
            });
            return View();
        }

        public void RefreshDataAndRedirect(string redirectUrl)
        {
            var mainWindow = Electron.WindowManager.BrowserWindows.Last();
            mainWindow.LoadURL($"http://localhost:{BridgeSettings.WebPort}/{redirectUrl}");
        }

    }
}
