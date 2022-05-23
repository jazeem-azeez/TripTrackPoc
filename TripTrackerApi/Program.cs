using DomainServices.Implementations;
using DomainServices.Interfaces;

using ExternalServices.Implementations;
using ExternalServices.Intefaces;

using Microsoft.AspNetCore.Mvc;

using PersistenceServices;

using Shared.DomainModels;

namespace TripTrackerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //builder.Services.AddVersionedApiExplorer(o =>
            //{
            //    o.GroupNameFormat = "'v'VVV";
            //    o.SubstituteApiVersionInUrl = true;
            //});
            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            builder.Services.AddSwaggerGen();
            builder.Services.AddPersistence();
            builder.Services.AddSingleton(typeof(ITruckService), typeof(TruckService));
            builder.Services.AddSingleton(typeof(ITripPlanService), typeof(TripPlanService));
            builder.Services.AddSingleton(typeof(IBizService<DriverDOM>), typeof(DriverService));
            builder.Services.AddSingleton(typeof(INominatimApiInterface), typeof(NominatimApiInterface));

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