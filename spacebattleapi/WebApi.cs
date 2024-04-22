using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MinimalHelpers.Routing;

namespace spacebattleapi
{
    public class WebApi
    {
        public static void Start(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();
            app.UseHttpsRedirection();

            app.MapEndpoints();

            app.RunAsync("http://localhost:7880");

            var clientHandler = new HttpClientHandler();
            var client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri("http://localhost:7880");

            var points = new Dictionary<string, object>
            {
                { "cmd", "testcommand" },
                { "gameId", 3474 },
                { "gg", 54 },
                { "hh", 666 },
                { "Speed", 666 },
                { "CanRotate", true }
            };

            var content = JsonContent.Create(points);
            client.PostAsync("/message", content);
        }
    }
}
