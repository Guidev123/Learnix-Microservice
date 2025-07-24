using Microsoft.AspNetCore.Routing;

namespace Learnix.Commons.WebApi.Endpoints
{
    public interface IEndpoint
    {
        void MapEndpoint(IEndpointRouteBuilder app);
    }
}