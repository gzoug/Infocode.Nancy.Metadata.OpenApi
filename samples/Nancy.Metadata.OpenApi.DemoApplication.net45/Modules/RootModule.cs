﻿using Nancy.Metadata.Modules;
using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.DemoApplication.net45.Model;
using Nancy.Metadata.OpenApi.Fluent;
using Nancy.ModelBinding;
using System.Linq;

namespace Nancy.Metadata.OpenApi.DemoApplication.net45.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule() : base("/api")
        {
            Get["SimpleRequest", "/hello"] = r => HelloWorld();
            Get["SimpleRequestWithParameter", "/hello/{name}"] = r => Hello(r.name);
            Get["SimpleRequestWithParameterArray", "/hello/{names}"] = r => Hello(r.names);
            Post["SimplePostRequest", "/hello"] = r => HelloPost();
            Post["PostRequestWithModel", "/hello/model"] = r => HelloModel();
            Post["PostRequestWithNestedModel", "/hello/nestedmodel"] = r => HelloNestedModel();
        }

        private Response HelloNestedModel()
        {
            NestedRequestModel model = this.Bind<NestedRequestModel>();

            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = $"Hello, {model.SimpleModel.Name}. We got your name from nested object"
            };

            return Response.AsJson(response);
        }

        private Response HelloModel()
        {
            SimpleRequestModel model = this.Bind<SimpleRequestModel>();

            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = $"Hello, {model.Name}"
            };

            return Response.AsJson(response);
        }

        private Response HelloPost()
        {
            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = "Hello Post!"
            };

            return Response.AsJson(response);
        }

        private Response Hello(string name)
        {
            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = $"Hello, {name}"
            };

            return Response.AsJson(response);
        }

        private Response Hello(string[] names)
        {
            var response = new SimpleResponseModel
            {
                Hello = names.Aggregate((curr, next) => string.Concat(curr, ", ", next))
            };

            return Response.AsJson(response);
        }

        private Response HelloWorld()
        {
            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = "Hello World!"
            };

            return Response.AsJson(response);
        }
    }

    public class RootMetadataModule : MetadataModule<OpenApiRouteMetadata>
    {
        public RootMetadataModule()
        {
            Describe["SimpleRequest"] = desc => new OpenApiRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithSummary("Simple GET example"));

            Describe["SimpleRequestWithParameter"] = desc => new OpenApiRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithRequestParameter("name")
                            .WithSummary("Simple GET with parameters"));

            Describe["SimpleRequestWithParameterArray"] = desc => new OpenApiRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                .WithRequestParameter("names", isArray: true, type: "string")
                .WithSummary("Simple GET with array parameters"));

            Describe["SimplePostRequest"] = desc => new OpenApiRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                    .WithSummary("Simple POST example"));

            Describe["PostRequestWithModel"] = desc => new OpenApiRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel))
                    .WithResponse("400", "Bad request")
                    .WithSummary("Simple POST example with request model")
                    .WithRequestModel(typeof(SimpleRequestModel)));

            Describe["PostRequestWithNestedModel"] = desc => new OpenApiRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel))
                    .WithResponse("400", "Bad request")
                    .WithSummary("Simple POST example with nested request model")
                    .WithRequestModel(typeof(NestedRequestModel)));
        }
    }
}