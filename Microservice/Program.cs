// See https://aka.ms/new-console-template for more information
using Amazon.Runtime;
using Domain;
using EasyNetQ;
using Messages;
using Microservice.Repository;
using Microservice.Seed;
using Microservice.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

internal partial class Program
{
    private static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureAppConfiguration(builder =>
        {
            builder.Sources.Clear();
            builder.AddConfiguration(configuration);
        });

        var mongoDbConnection = $"mongodb://root:mongopw@{configuration["MongoDbHost"]}";
        var rabbitMqConnection = $"host={configuration["RabbitMQHost"]}";

        Console.WriteLine(mongoDbConnection);
        Console.WriteLine(rabbitMqConnection);

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                return new MongoClient(mongoDbConnection);
            });

            services.AddSingleton<IBus>(RabbitHutch.CreateBus(rabbitMqConnection));
            services.AddScoped<ISeedService, SeedService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPriceReductionService, PriceReductionService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPriceReductionRepository, PriceReductionRepository>();
        });

        using IHost host = builder.Build();

        using var serviceScope = host.Services.CreateScope();
        var provider = serviceScope.ServiceProvider;

        var seedService = provider.GetRequiredService<ISeedService>();
        seedService.PopulateDataAsync();

        var bus = provider.GetRequiredService<IBus>();
        bus.Rpc.RespondAsync<ProductsRequest, ProductsResponse>(async request => await provider.GetRequiredService<IProductService>().GetProductsAsync());
        bus.Rpc.RespondAsync<ProductDetailsRequest, ProductDetailsResponse>(async request => await provider.GetRequiredService<IProductService>().GetProductAsync(request.ProductId));

        host.Run();
        
        bus.Dispose();
    }
}