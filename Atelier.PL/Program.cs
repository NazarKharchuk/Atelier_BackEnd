using Atelier.BLL.DTO;
using Atelier.BLL.Interfaces;
using Atelier.BLL.Mapping;
using Atelier.BLL.Services;
using Atelier.DAL.Context;
using Atelier.DAL.Interfaces;
using Atelier.DAL.Repositories;
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

            var optionsBuilder = new DbContextOptionsBuilder<AtelierContext>();

            Console.WriteLine("DefaultConnection ==== " + builder.Configuration.GetConnectionString("DefaultConnection"));
            
            var options = optionsBuilder
                    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .Options;

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(s => new UnitOfWork(options));

            builder.Services.AddAutoMapper(typeof(WorksTypeProfile), typeof(WorksTypeModelProfile),
                typeof(FilteredListRequestModelPrifile), typeof(ResponseIdAndStringModelProfile));

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