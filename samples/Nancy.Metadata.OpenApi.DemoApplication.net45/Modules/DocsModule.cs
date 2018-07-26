using Infocode.Nancy.Metadata.OpenApi.Model;
using Infocode.Nancy.Metadata.OpenApi.Modules;
using Nancy;
using Nancy.Routing;

namespace Infocode.Nancy.Metadata.OpenApi.DemoApplication.net45.Modules
{
    public class DocsModule : OpenApiDocsModuleBase
    {
        public static Server Server => new Server() { Description = "My Description", Url = "http://localhost:5000/" };

        public DocsModule(IRouteCacheProvider routeCacheProvider) :
        base(routeCacheProvider,
        "/api/docs/openapi.json",
        "Sample API documentation",
        "v1.0",
        host: Server,
        apiBaseUrl: "/api")
        {
            Get["/"] = _ => Response.AsRedirect("/Content/index.html");
        }
    }
}
