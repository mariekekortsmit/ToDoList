using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ToDoList
{
    public static class MediatrExtensions
    {
        public static RouteHandlerBuilder MapGet<TRequest, TResponse>(this IEndpointRouteBuilder app, string template)
            where TRequest : IRequest<TResponse>
        {
            return app.MapGet(template,
                async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request));
        }

        public static RouteHandlerBuilder MapPost<TRequest, TResponse>(this IEndpointRouteBuilder app, string template)
           where TRequest : IRequest<TResponse>
        {
            return app.MapPost(template,
                async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request));
        }

        public static RouteHandlerBuilder MapPut<TRequest, TResponse>(this IEndpointRouteBuilder app, string template)
        where TRequest : IRequest<TResponse>
        {
            return app.MapPut(template,
                async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request));
        }

        public static RouteHandlerBuilder MapDelete<TRequest, TResponse>(this IEndpointRouteBuilder app, string template)
        where TRequest : IRequest<TResponse>
        {
            return app.MapDelete(template,
                async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request));
        }
    }
}
