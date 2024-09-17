using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;
using System.Text.Json;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication app)
        {
            var name = "MongoDbConnection";
            var connection = app.Configuration.GetConnectionString(name);
            await DB.InitAsync("SearchDb",
                MongoClientSettings.FromConnectionString(connection));

            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            var count = await DB.CountAsync<Item>();

            using var scope = app.Services.CreateScope();

            var httpClient = scope.ServiceProvider.GetService<AuctionSvcHttpClient>();

            var items = await httpClient.GetItemsFromSearchDb();

            Console.WriteLine(items.Count + " Items returned from the auction service");

            if (items.Count > 0)
            {
                await DB.SaveAsync(items);
            }

            #region
            /*if (count == 0)
            {
                Console.WriteLine("--> No data - attempt to seed some data");

                var itemData = await File
                    .ReadAllTextAsync("Data/auction.json");

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var items = JsonSerializer
                    .Deserialize<List<Item>>(itemData, options);

                await DB.SaveAsync(items);                
            }*/
            #endregion
        }
    }
}
