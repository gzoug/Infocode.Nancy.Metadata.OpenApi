using System;
using Infocode.Nancy.Metadata.OpenApi.Core;
using Infocode.Nancy.Metadata.OpenApi.Model;

namespace Infocode.Nancy.Metadata.OpenApi.Fluent
{
    public static class OpenApiRouteMetadataExtensions
    {
        /// <summary>
        /// Generate a new endpoint reoute which will add request / response objects.
        /// </summary>
        /// <param name="routeMetadata"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static OpenApiRouteMetadata With(this OpenApiRouteMetadata routeMetadata,
            Func<Endpoint, Endpoint> info)
        {
            routeMetadata.Info = info(routeMetadata.Info ?? new Endpoint(routeMetadata.Name));

            return routeMetadata;
        }
    }
}
