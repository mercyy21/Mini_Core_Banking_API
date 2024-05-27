using Microsoft.OpenApi.Models;
using Application.AutoMapperConfig;
using System.Reflection;
using Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Application.Customers.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Customers.BehaviourPipeline;
using Application.Customers.Jwt;
using Application.UtilityService;
using Application.Interfaces;
using Infrastructure.UtilityService;
using Infrastructure.PasswordHasher;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option => option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            });
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddScoped<IMiniCoreBankingDbContext, MiniCoreBankingDbContext>();
            builder.Services.AddScoped<IMiniCoreBankingDbContext, MiniCoreBankingDbContext>();
            builder.Services.AddScoped<IJwtToken, JWTToken>();
            builder.Services.AddSingleton<IHasher, Hasher>();
            builder.Services.AddScoped<IDecrypt, DecryptService>();
            builder.Services.AddSingleton<IEncrypt, EncryptionService>();
            builder.Services.AddDbContext<MiniCoreBankingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                builder.Services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(assembly);
                    cfg.AddOpenBehavior(typeof(ValidationBehaviourPipeline<,>));
                });
            }
            builder.Services.AddHttpContextAccessor();
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
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
                });
            });


            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}