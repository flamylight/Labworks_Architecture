using Autofac;
using Autofac.Extensions.DependencyInjection;
using BLL.Interfaces;
using BLL.Managers;
using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Host.ConfigureContainer<ContainerBuilder>(c =>
    {
        //Repositories
        c.RegisterGeneric(typeof(GenericRepository<>))
            .As(typeof(IGenericRepository<>))
            .InstancePerLifetimeScope();
        
        c.RegisterType<OrderRepository>()
            .As<IGenericRepository<Order>>()
            .InstancePerLifetimeScope();
        
        c.RegisterType<PackageRepository>()
            .As<IGenericRepository<Package>>()
            .InstancePerLifetimeScope();
        
        //Unit of Work
        c.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
        
        //Managers
        c.RegisterType<OrderManager>()
            .As<IOrderManager>()
            .InstancePerLifetimeScope();
        
        c.RegisterType<PackageManager>()
            .As<IPackageManager>()
            .InstancePerLifetimeScope();
        
        c.RegisterType<ServiceManager>()
            .As<IServiceManager>()
            .InstancePerLifetimeScope();
    }
);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => 
        options.SwaggerEndpoint("/openapi/v1.json", "Design studio"));   
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();