using Microsoft.OpenApi.Models;
using Application.AutoMapperConfig;
using Application.Customers.CustomerCommand;
using System.Reflection;
using Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Application.Customers.Validator;
using Mini_Core_Banking_Project;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
builder.Services.AddControllers();
builder.Services.AddScoped<IMiniCoreBankingDbContext, MiniCoreBankingDbContext>();
builder.Services.AddDbContext<MiniCoreBankingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();
//builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviourPipeline<,>));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateCustomerCommandHandler>());
foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblies(assembly);
        cfg.AddOpenBehavior(typeof(ValidationBehaviourPipeline<,>));
    });
}
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Swagger Documentation Section
var info = new OpenApiInfo()
{
    Title = "Mini Core Banking API",
    Version = "v1",
    Description = "An ASP.NET Core Web API for a Mini Core Banking Application",
    Contact = new OpenApiContact()
    {
        Name = "Awopetu Mercy",
        Email = "mawopetu21@gmail.com",
    }

};

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", info);

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
