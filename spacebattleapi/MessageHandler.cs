using Hwdtech;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MinimalHelpers.Routing;

namespace spacebattle
{
    public class MessageHandler : IEndpointRouteHandlerBuilder
    {
        public void MapEndpoints(IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/message", Insert);
        }

        public static IResult Insert(Message message)
        {
            try
            {
                var gameId = message.gameId;
                var threadId = IoC.Resolve<Guid>("Server.GetThreadIdByGameId", gameId);
                var command = IoC.Resolve<ICommand>("Server.BuildToGameCommand", message);
                IoC.Resolve<ICommand>("Server.SendCommand", threadId, command).Execute();

                return Results.Accepted();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Results.BadRequest(e.Message);
            }
        }

        public static void SetScope(object scope)
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        }
    }
}
