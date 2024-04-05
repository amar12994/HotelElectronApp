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
        public static T JsonFileReadAsync<T>()
        {
            using FileStream stream = File.OpenRead($"{localAapDataPath}\\data.json");
            return JsonSerializer.Deserialize<T>(stream);
        }
        public static async Task WriteToFileAsync(List<Category> cetegories)
        {
            if (!Directory.Exists(localAapDataPath))
            {
                Directory.CreateDirectory(localAapDataPath);
            }
            await using FileStream createStream = File.Create($"{localAapDataPath}\\data.json");
            await JsonSerializer.SerializeAsync(createStream, cetegories);
        }
    }
}