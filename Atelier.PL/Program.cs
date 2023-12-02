using Atelier.BLL.DTO;
using Atelier.BLL.Interfaces;
using Atelier.BLL.Mapping;
using Atelier.BLL.Services;
using Atelier.DAL.Context;
using Atelier.DAL.Interfaces;
using Atelier.DAL.Repositories;
using Atelier.PL.Mapping.Client;
using Atelier.PL.Mapping.Material;
using Atelier.PL.Mapping.Request;
using Atelier.PL.Mapping.WorksType;
using Microsoft.EntityFrameworkCore;

namespace Atelier.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddScoped<IWorksTypeService<WorksTypeDTO>, WorksTypeService>();
            builder.Services.AddScoped<IClientService<ClientDTO>, ClientService>();
            builder.Services.AddScoped<IMaterialService<MaterialDTO>, MaterialService>();

            var optionsBuilder = new DbContextOptionsBuilder<AtelierContext>();
            
            var options = optionsBuilder
                    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .Options;

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(s => new UnitOfWork(options));

            builder.Services.AddAutoMapper(typeof(ClientProfile), typeof(MaterialProfile),
                typeof(WorksTypeProfile), typeof(MaterialModelProfile), typeof(ClientModelProfile), 
                typeof(WorksTypeModelProfile), typeof(FilteredClientListRequestModelPrifile), 
                typeof(FilteredMaterialListRequestModelPrifile), typeof(FilteredListRequestModelPrifile), 
                typeof(ResponseIdAndStringModelProfile));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
        }
    }
}