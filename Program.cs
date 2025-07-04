using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using NSwag.Generation.AspNetCore;   // <-- para AddOpenApiDocument()

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "MovieAPI";
    config.Title        = "Movie API v1";
    config.Version      = "v1";
    
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    app.UseOpenApi();

    app.UseSwaggerUi();
}



app.MapControllers();
app.Run();