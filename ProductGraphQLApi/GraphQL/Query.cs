using Grpc.Core;
using HotChocolate;
using ProductGrpcService;

namespace ProductGraphQLApi.GraphQL;

public sealed class Query
{
    [GraphQLName("products")]
    public async Task<IReadOnlyList<Product>> GetProductsAsync(
        [Service] ProductService.ProductServiceClient client,
        CancellationToken cancellationToken,
        string? category = null)
    {
        try
        {
            using var call = client.GetProductsAsync(new GetProductsRequest(),
                deadline: DateTime.UtcNow.AddSeconds(5), cancellationToken: cancellationToken);
            var response = await call.ResponseAsync;
            return response.Products.Where(product => string.IsNullOrWhiteSpace(category)
                || string.Equals(product.Category, category.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }
        catch (RpcException exception)
        {
            throw ToGraphQLError(exception);
        }
    }

    [GraphQLName("productById")]
    public async Task<Product?> GetProductByIdAsync(
        int id,
        [Service] ProductService.ProductServiceClient client,
        CancellationToken cancellationToken)
    {
        try
        {
            using var call = client.GetProductByIdAsync(new GetProductRequest { Id = id },
                deadline: DateTime.UtcNow.AddSeconds(5), cancellationToken: cancellationToken);
            return await call.ResponseAsync;
        }
        catch (RpcException exception) when (exception.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
        catch (RpcException exception)
        {
            throw ToGraphQLError(exception);
        }
    }

    private static GraphQLException ToGraphQLError(RpcException exception)
    {
        var (message, code) = exception.StatusCode switch
        {
            StatusCode.DeadlineExceeded => ("The product service took too long to respond.", "PRODUCT_SERVICE_TIMEOUT"),
            StatusCode.Cancelled => ("The product request was cancelled.", "REQUEST_CANCELLED"),
            _ => ("The product service is currently unavailable.", "PRODUCT_SERVICE_UNAVAILABLE")
        };
        return new GraphQLException(ErrorBuilder.New().SetMessage(message).SetCode(code).Build());
    }
}
