using HotChocolate.AspNetCore;
using ProductGrpcService;
using ProductGraphQLApi.GraphQL;

var builder = WebApplication.CreateBuilder(args);
var grpcUrl = builder.Configuration["GrpcService:Url"]
    ?? throw new InvalidOperationException("GrpcService:Url is required.");
builder.Services.AddGrpcClient<ProductService.ProductServiceClient>(options =>
    options.Address = new Uri(grpcUrl));
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<ProductType>()
    .ModifyRequestOptions(options => options.IncludeExceptionDetails = false);
var app = builder.Build();
app.MapGraphQL("/graphql").WithOptions(new GraphQLServerOptions
{
    Tool =
    {
        Enable = app.Environment.IsDevelopment(),
        ServeMode = GraphQLToolServeMode.Embedded
    }
});
app.Run();
