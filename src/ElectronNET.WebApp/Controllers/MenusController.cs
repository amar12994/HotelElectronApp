using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ElectronNET.API.Entities;
using ElectronNET.API;

namespace ElectronNET.WebApp.Controllers
{
    public class MenusController : Controller
    {
        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.App.Ready += () => CreateContextMenu();

                var menu = new API.Entities.MenuItem[] {
                new API.Entities.MenuItem { Label = "Edit", Submenu = new API.Entities.MenuItem[] {
                    new API.Entities.MenuItem { Label = "Undo", Accelerator = "CmdOrCtrl+Z", Role = MenuRole.undo },
                    new API.Entities.MenuItem { Label = "Redo", Accelerator = "Shift+CmdOrCtrl+Z", Role = MenuRole.redo },
                    new API.Entities.MenuItem { Type = MenuType.separator },
                    new API.Entities.MenuItem { Label = "Cut", Accelerator = "CmdOrCtrl+X", Role = MenuRole.cut },
                    new API.Entities.MenuItem { Label = "Copy", Accelerator = "CmdOrCtrl+C", Role = MenuRole.copy },
                    new API.Entities.MenuItem { Label = "Paste", Accelerator = "CmdOrCtrl+V", Role = MenuRole.paste },
                    new API.Entities.MenuItem { Label = "Select All", Accelerator = "CmdOrCtrl+A", Role = MenuRole.selectall }
                }
                },
                new API.Entities.MenuItem { Label = "View", Submenu = new API.Entities.MenuItem[] {
                    new API.Entities.MenuItem
                    {
                        Label = "Reload",
                        Accelerator = "CmdOrCtrl+R",
                        Click = () =>
                        {
                            // on reload, start fresh and close any old
                            // open secondary windows
                            var mainWindowId = Electron.WindowManager.BrowserWindows.ToList().First().Id;
                            Electron.WindowManager.BrowserWindows.ToList().ForEach(browserWindow => {
                                if(browserWindow.Id != mainWindowId)
                                {
                                    browserWindow.Close();
                                }
                                else
                                {
                                    browserWindow.Reload();
                                }
                            });
                        }
                    },
                    new API.Entities.MenuItem
                    {
                        Label = "Toggle Full Screen",
                        Accelerator = "CmdOrCtrl+F",
                        Click = async () =>
                        {
                            bool isFullScreen = await Electron.WindowManager.BrowserWindows.First().IsFullScreenAsync();
                            Electron.WindowManager.BrowserWindows.First().SetFullScreen(!isFullScreen);
                        }
                    },
                    new API.Entities.MenuItem
                    {
                        Label = "Open Developer Tools",
                        Accelerator = "CmdOrCtrl+I",
                        Click = () => Electron.WindowManager.BrowserWindows.First().WebContents.OpenDevTools()
                    },
                    new API.Entities.MenuItem
                    {
                        Type = MenuType.separator
                    },
                    new API.Entities.MenuItem
                    {
                        Label = "App Menu Demo",
                        Click = async () => {
                            var options = new MessageBoxOptions("This demo is for the Menu section, showing how to create a clickable menu item in the application menu.");
                            options.Type = MessageBoxType.info;
                            options.Title = "Application Menu Demo";
                            await Electron.Dialog.ShowMessageBoxAsync(options);
                        }
                    }
                }
                },
                new API.Entities.MenuItem { Label = "Window", Role = MenuRole.window, Submenu = new API.Entities.MenuItem[] {
                     new API.Entities.MenuItem { Label = "Minimize", Accelerator = "CmdOrCtrl+M", Role = MenuRole.minimize },
                     new API.Entities.MenuItem { Label = "Close", Accelerator = "CmdOrCtrl+W", Role = MenuRole.close }
                }
                },
                new API.Entities.MenuItem { Label = "Help", Role = MenuRole.help, Submenu = new API.Entities.MenuItem[] {
                    new API.Entities.MenuItem
                    {
                        Label = "Learn More",
                        Click = async () => await Electron.Shell.OpenExternalAsync("https://github.com/ElectronNET")
                    }
                }
                }
            };

                Electron.Menu.SetApplicationMenu(menu);

            }

            return View();
        }

        private void CreateContextMenu()
        {
            var menu = new API.Entities.MenuItem[]
            {
                new API.Entities.MenuItem
                {
                    Label = "Hello",
                    Click = async () => await Electron.Dialog.ShowMessageBoxAsync("Electron.NET rocks!")
                },
                new API.Entities.MenuItem { Type = MenuType.separator },
                new API.Entities.MenuItem { Label = "Electron.NET", Type = MenuType.checkbox, Checked = true }
            };

            var mainWindow = Electron.WindowManager.BrowserWindows.FirstOrDefault();
            Electron.Menu.SetContextMenu(mainWindow, menu);

            Electron.IpcMain.On("show-context-menu", (args) =>
            {
                var mainWindow = Electron.WindowManager.BrowserWindows.FirstOrDefault();
                Electron.Menu.ContextMenuPopup(mainWindow);
            });
        }
    }
}