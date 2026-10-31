using HotChocolate.Types;
using ProductGrpcService;

namespace ProductGraphQLApi.GraphQL;

public sealed class ProductType : ObjectType<Product>
{
    protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
    {
        descriptor.Name("Product");
        // Expose only domain fields, not protobuf serialization members.
        descriptor.BindFieldsExplicitly();
        descriptor.Field(p => p.Id).Name("id").Type<NonNullType<IntType>>();
        descriptor.Field(p => p.Name).Name("name").Type<NonNullType<StringType>>();
        descriptor.Field(p => p.Description).Name("description").Type<NonNullType<StringType>>();
        descriptor.Field(p => p.Price).Name("price").Type<NonNullType<FloatType>>();
        descriptor.Field(p => p.Category).Name("category").Type<NonNullType<StringType>>();
        descriptor.Field(p => p.StockQuantity).Name("stockQuantity").Type<NonNullType<IntType>>();
    }
}
