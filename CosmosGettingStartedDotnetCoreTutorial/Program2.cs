using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SqlInjectionExample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<CosmosClient>(InitializeCosmosClientInstanceAsync().GetAwaiter().GetResult());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task<CosmosClient> InitializeCosmosClientInstanceAsync()
        {
            string connectionString = "your_cosmos_db_connection_string_here";
            CosmosClient client = new CosmosClient(connectionString);
            await Task.CompletedTask;
            return client;
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public UserController(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer("your_database_name", "your_container_name");
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string nome)
        {
            // Vulnerável a SQL Injection
            var query = $"SELECT * FROM c WHERE c.name = '{nome}'";
            var queryDefinition = new QueryDefinition(query);
            var users = new List<dynamic>();

            using (FeedIterator<dynamic> resultSet = _container.GetItemQueryIterator<dynamic>(queryDefinition))
            {
                while (resultSet.HasMoreResults)
                {
                    FeedResponse<dynamic> response = await resultSet.ReadNextAsync();
                    users.AddRange(response);
                }
            }

            return Ok(users);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}