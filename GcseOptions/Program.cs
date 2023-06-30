using GcseOptions.Data;
using Microsoft.Extensions.Configuration;

namespace GcseOptions

{
    internal class Program
    {
        static void Main()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
            string connectionString = config["ConnectionStrings:carbonDb"] ?? string.Empty;
            Console.WriteLine(connectionString);
            GcseOptionsDb db = new(connectionString);

            
            db.Menu();
        }
    }
}