using ProductGrpcService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
var app = builder.Build();
app.MapGrpcService<ProductServiceImpl>();
if (app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();
app.Run();
