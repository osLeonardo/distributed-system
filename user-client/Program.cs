using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.OpenApi.Models;
using System.Net.Http;
using System.Text;
using user_client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserClient API", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddGrpcClient<LocationService.LocationServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:8080");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return new GrpcWebHandler(GrpcWebMode.GrpcWeb, handler);
});

builder.Services.AddGrpcClient<ProductService.ProductServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:8080");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return new GrpcWebHandler(GrpcWebMode.GrpcWeb, handler);
});

builder.Services.AddGrpcClient<ProductLocationService.ProductLocationServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:8080");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return new GrpcWebHandler(GrpcWebMode.GrpcWeb, handler);
});

builder.Services.AddSingleton<UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseSession();

app.Run();