
using IpBlockApi.Repository;
using IpBlockApi.Services;

namespace IpBlockApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // add httpclient 
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IGeoService, GeoRepository>();
            // add singelton becuse data in memory 
            builder.Services.AddSingleton<IBlockedCountryRepository, BlockedCountryRepository>();
            
            builder.Services.AddSingleton<IBlockedAttemptLogger, InMemoryBlockedAttemptLogger>();
            builder.Services.AddSingleton<ITemporaryBlockService, InMemoryTemporaryBlockService>();
            builder.Services.AddHostedService<ExpiredBlockCleanupService>();





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
