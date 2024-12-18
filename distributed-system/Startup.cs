﻿using distributed_system.Context;
using distributed_system.Repositories;
using distributed_system.Repositories.Intefaces;
using distributed_system.Services;
using Microsoft.EntityFrameworkCore;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddLogging();
        services.AddDbContext<InventoryContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5432;Database=distributed-system;Username=postgres;Password=postgres"));
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductLocationRepository, ProductLocationRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseGrpcWeb();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<LocationServiceImpl>().EnableGrpcWeb();
            endpoints.MapGrpcService<ProductServiceImpl>().EnableGrpcWeb();
            endpoints.MapGrpcService<ProductLocationServiceImpl>().EnableGrpcWeb();

            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Servidor gRPC esta em execucao");
            });
        });
    }
}
