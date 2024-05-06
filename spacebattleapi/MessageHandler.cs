using Hwdtech;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MinimalHelpers.Routing;

namespace spacebattle
{
    public class MessageHandler : IEndpointRouteHandlerBuilder
    {
        private static object? _scope;
        public void MapEndpoints(IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/message", Insert);
        }

        public static IResult Insert(Message message)
        {
            try
            {
                if (_scope != null)
                {
                    IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();
                }

                var gameId = message.gameId;
                var threadId = IoC.Resolve<Guid>("Server.GetThreadIdByGameId", gameId);
                var command = IoC.Resolve<ICommand>("Server.BuildToGameCommand", message);
                IoC.Resolve<ICommand>("Server.SendCommand", threadId, command).Execute();

                return Results.Accepted();
            }
            catch (Exception e)
            {
                IoC.Resolve<ICommand>("Exception.Handler", e);
                return Results.BadRequest();
            }
        }

        public static void SetScope(object scope)
        {
            _scope = scope;
        }
    }
}
