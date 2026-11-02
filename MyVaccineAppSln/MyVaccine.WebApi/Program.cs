using FluentValidation;
using MyVaccine.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var configuration = builder.Configuration;

builder.Services.SetDatabaseConfiguration(configuration);
builder.Services.SetMyVaccineAuthConfiguration(configuration);
builder.Services.SetDependencyInjection();
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

//ESTE CODIGO ES  UTILIZADO PARA OBLIGAR AL SERVICIO A FUNCIONAR LOCALMENTE EN EL PUERTO 38791 PERO SI TIENES EL CONTENEDOR
// Y EL SERVICIO VA A HABER CONFLICTO  POR QUE USAN EL MISMO PUERTO ENTONCES EN ESE CASO NO ES NECESARIO 

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(38791); // Cambia a ListenAnyIP para asegurarte de que pueda aceptar peticiones externas
//});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyVaccineAppDbContext>();
    try
    {
        // Aplica las migraciones pendientes
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
        // Considera si deberías detener el arranque de la aplicaci�n aquí o no.
    }
}

// ── Middleware Pipeline ──
// Swagger activado para que funcione tanto en local como en Docker (Release/Development)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyVaccine API V1");
    c.RoutePrefix = string.Empty; // Esto hace que Swagger abra directamente en la raíz (http://localhost:38791/)
});

//app.MapGet("/", () => Results.Redirect("/swagger"));

//app.UseHttpsRedirection();
app.UseDeveloperExceptionPage();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
