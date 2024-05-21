using ElectronNET.WebApp.Models;
using System;
using System.Collections.Generic;

namespace ElectronNET.WebApp.Domain
{
    public class ElectronD
    {
        private static ElectronD _instance;

        private ElectronD() { }

        public static ElectronD Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ElectronD();

                return _instance;
            }
        }
        public List<Category> Cetegories { get; set; } = new();
        public List<CustomerOrder> CustomerOrders { get; set; } = new();
        public List<CartMenuItem> CartMenuItems { get; set; }
        public string ToastrMessage { get; set; }
        public string ImageBasePath { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}\\Image";
    }
}