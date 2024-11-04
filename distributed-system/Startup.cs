using distributed_system.Context;
using distributed_system.Services;
using Microsoft.EntityFrameworkCore;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddDbContext<InventoryContext>(options =>
            options.UseNpgsql(Program.Configuration.GetConnectionString("PostgresConnection")));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<LocationServiceImpl>();

            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Servidor gRPC está em execucao");
            });
        });
    }
}
