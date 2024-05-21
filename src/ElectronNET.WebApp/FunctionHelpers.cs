using ElectronNET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ElectronNET.WebApp
{

    public static class FunctionHelpers
    {
        private static string localAapDataPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}";
        public static T JsonFileReadAsync<T>(string fileName)
        {
            string fileNameWithCompletePath = $"{localAapDataPath}\\{fileName}";
            if (!File.Exists(fileNameWithCompletePath))
            {
                return default;
            }
            using FileStream stream = File.OpenRead(fileNameWithCompletePath);
            return JsonSerializer.Deserialize<T>(stream);
        }
        public static async Task WriteCategoryMenuItemsToFileAsync(List<Category> cetegories)
        {
            if (!Directory.Exists(localAapDataPath))
            {
                Directory.CreateDirectory(localAapDataPath);
            }
            await using FileStream createStream = File.Create($"{localAapDataPath}\\data.json");
            await JsonSerializer.SerializeAsync(createStream, cetegories);
        }

        public static async Task WriteCustomerOrderToFileAsync(List<CustomerOrder> customerOrders)
        {
            if (!Directory.Exists(localAapDataPath))
            {
                Directory.CreateDirectory(localAapDataPath);
            }
            await using FileStream createStream = File.Create($"{localAapDataPath}\\CustomerOrder.json");
            await JsonSerializer.SerializeAsync(createStream, customerOrders);
        }

        public static string CopyFile(string sourcePath)
        {
            string directory = $"{localAapDataPath}\\Image";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filename = Path.GetFileName(sourcePath);
            string destinationPath = $"{directory}\\{Guid.NewGuid()}_{filename}";
            FileInfo fi1 = new(sourcePath);
            fi1.CopyTo(destinationPath);
            return Path.GetFileName(destinationPath);
            //return destinationPath;
        }
    }
}