using MyWebNovel.Api.Endpoints;

namespace MyWebNovel.Api.Extensions
{
    public static class EndpointMappingExtensions
    {
        public static void MapAllEndpoints(this WebApplication app)
        {
            app.MapAuthEndpoints();
            app.MapNovelEndpoints();
            app.MapTagEndpoints();
        }
    }
}
