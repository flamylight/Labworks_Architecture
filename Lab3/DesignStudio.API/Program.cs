using Autofac;
using Autofac.Extensions.DependencyInjection;
using BLL.DependencyInjection;
using BLL.Mapping;
using DAL.Data;
using DAL.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ExceptionsHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(cfg => { }, typeof(OrderProfile));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Host.ConfigureContainer<ContainerBuilder>(c =>
{
    c.RegisterModule(new DALModule());
    c.RegisterModule(new BLLModule());
});

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

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();