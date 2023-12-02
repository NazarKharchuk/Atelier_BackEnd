using Atelier.BLL.DTO;
using Atelier.BLL.Interfaces;
using Atelier.BLL.Mapping;
using Atelier.BLL.Services;
using Atelier.DAL.Context;
using Atelier.DAL.Interfaces;
using Atelier.DAL.Repositories;
using Atelier.PL.Mapping.Client;
using Atelier.PL.Mapping.Employee;
using Atelier.PL.Mapping.Material;
using Atelier.PL.Mapping.Request;
using Atelier.PL.Mapping.User;
using Atelier.PL.Mapping.WorksType;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Atelier.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            builder.Services.AddControllers();

            builder.Services.AddScoped<IWorksTypeService<WorksTypeDTO>, WorksTypeService>();
            builder.Services.AddScoped<IClientService<ClientDTO>, ClientService>();
            builder.Services.AddScoped<IMaterialService<MaterialDTO>, MaterialService>();
            builder.Services.AddScoped<IUserService<UserDTO>, UserService>();
            builder.Services.AddScoped<IEmployeeService<EmployeeDTO>, EmployeeService>();

            var optionsBuilder = new DbContextOptionsBuilder<AtelierContext>();
            
            var options = optionsBuilder
                    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .Options;

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(s => new UnitOfWork(options));

            builder.Services.AddAutoMapper(typeof(UserProfile), typeof(ClientProfile), typeof(EmployeeProfile), typeof(MaterialProfile),
                typeof(WorksTypeProfile), typeof(UserLoginModelProfile), typeof(MaterialModelProfile), typeof(EmployeeModelProfile),
                typeof(EmployeeRegisterModelProfile), typeof(ClientModelProfile), typeof(WorksTypeModelProfile),
                typeof(FilteredClientListRequestModelPrifile), typeof(FilteredMaterialListRequestModelPrifile),
                typeof(FilteredListRequestModelPrifile), typeof(ResponseIdAndStringModelProfile), typeof(AuthorizationResponseModelProfile));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JWT Token Authentication API",
                    Description = ".NET Web API"
                });
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

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