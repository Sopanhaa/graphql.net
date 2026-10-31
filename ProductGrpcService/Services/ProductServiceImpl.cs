using Grpc.Core;

namespace ProductGrpcService.Services;

public sealed class ProductServiceImpl : ProductService.ProductServiceBase
{
    private static readonly Product[] Products =
    [
        new() { Id = 1, Name = "Wireless Mouse", Description = "Ergonomic Bluetooth mouse", Price = 24.99, Category = "Electronics", StockQuantity = 45 },
        new() { Id = 2, Name = "Mechanical Keyboard", Description = "Compact keyboard with tactile switches", Price = 79.99, Category = "Electronics", StockQuantity = 20 },
        new() { Id = 3, Name = "USB-C Hub", Description = "Seven-port USB-C adapter", Price = 39.99, Category = "Electronics", StockQuantity = 30 },
        new() { Id = 4, Name = "Clean Code", Description = "A guide to writing readable software", Price = 34.99, Category = "Books", StockQuantity = 18 },
        new() { Id = 5, Name = "The Pragmatic Programmer", Description = "Practical software development advice", Price = 42.5, Category = "Books", StockQuantity = 12 },
        new() { Id = 6, Name = "Learning C#", Description = "Beginner programming workbook", Price = 29.99, Category = "Books", StockQuantity = 25 },
        new() { Id = 7, Name = "Desk Lamp", Description = "Adjustable LED desk lamp", Price = 22.99, Category = "Home", StockQuantity = 35 },
        new() { Id = 8, Name = "Stainless Steel Bottle", Description = "Reusable 750 ml water bottle", Price = 16.99, Category = "Home", StockQuantity = 60 },
    ];

    public override Task<GetProductsResponse> GetProducts(
        GetProductsRequest request, ServerCallContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        var response = new GetProductsResponse();
        // Clone mutable protobuf messages so callers never share the sample objects.
        response.Products.AddRange(Products.Select(product => product.Clone()));
        return Task.FromResult(response);
    }

    public override Task<Product> GetProductById(
        GetProductRequest request, ServerCallContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        var product = Products.FirstOrDefault(product => product.Id == request.Id);
        if (product is null)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Product with ID {request.Id} was not found."));
        return Task.FromResult(product.Clone());
    }
}
