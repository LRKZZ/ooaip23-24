using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MinimalHelpers.Routing;
using spacebattle;

namespace spacebattleapi
{
    public class ApiBuilder
    {
        public static void Build(object args)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddEndpointsApiExplorer();

            MessageHandler.SetScope(args);

            var app = builder.Build();
            app.UseHttpsRedirection();

            app.MapEndpoints();

            app.RunAsync("http://localhost:7881");
        }
    }
}
