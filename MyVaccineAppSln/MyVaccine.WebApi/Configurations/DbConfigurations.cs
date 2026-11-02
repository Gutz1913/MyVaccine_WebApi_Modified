using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Configurations;

public static class DbConfigurations
{
    public static IServiceCollection SetDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        //var connectionString = Environment.GetEnvironmentVariable(MyVaccineLiterals.CONNECTION_STRING);
        //var connectionString = "Server=localhost;Database=MyVaccineAppDb;User Id=sa;Password=abc123;TrustServerCertificate=True;";
        //  var connectionString = "Server=api-myvaccine.mymetalevents.com;Database=myvaccione_db;User Id=app_myvaccine_user;Password=MyVaccine.2024.!;TrustServerCertificate=True;";
        services.AddDbContext<MyVaccineAppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}