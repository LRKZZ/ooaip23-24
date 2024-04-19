using Hwdtech;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
namespace spacebattle;

public class Endpoint : IDisposable
{
    private static object _scope;
    private static string _port;
    private static WebApplication? _app;

    public void Run()
    {
        var webApplicationBuilder = WebApplication.CreateBuilder();
        _app = webApplicationBuilder.Build();
        _app.UseHttpsRedirection();
        _app.Map("/message", (Message message) =>
        {
            try
            {
                //Console.WriteLine(JsonSerializer.Serialize(message));
                // Console.WriteLine(message.cmd);
                // Console.WriteLine(message.gameId);
                // foreach (var tmp in message._additionalData.Values.ToArray())
                // {
                //     Console.WriteLine(tmp);
                // }
                //отправляет команду в поток
                if (_scope != null)
                {
                    IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();
                }

                var gameId = message.gameId;
                var threadId = IoC.Resolve<Guid>("Server.GetThreadIdByGameId", gameId);
                var command = IoC.Resolve<ICommand>("Server.BuildToGameCommand", message);
                IoC.Resolve<ICommand>("Server.SendCommand", threadId, command).Execute();
                return Results.Ok(message);
            }
            catch
            {
                return Results.BadRequest();
            }
        });
        _app.RunAsync($"http://localhost:{_port}");
    }
    public static void Stop()
    {
        if (_app != null)
        {
            _app.StopAsync();
        }
    }

    public void Dispose()
    {
        Stop();
    }

    public Endpoint(string port, object scope)
    {
        _port = port;
        _scope = scope;
    }
}
