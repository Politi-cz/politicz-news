﻿var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Configuration.AddEnvironmentVariables("NewsApi_");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services
    .AddNewsOptions(builder.Configuration)
    .AddSecurity(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddCors()
    .AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "News",
        Version = "v1",
        Description = "API providing interface for operations related to news",
        Contact = new OpenApiContact { Url = new Uri("https://github.com/PetrKoller") },
    }))
    .AddDatabase()
    .AddMediator(o =>
    {
        o.Namespace = "Politicz.News.Mediator";
        o.ServiceLifetime = ServiceLifetime.Transient;
    })
    .AddValidation();

var app = builder.Build();
app.UseSerilogRequestLogging();

app.UseCors(c => c
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader())
    .UseMiddleware<ExceptionHandlingMiddleware>();

app.MapNews();

if (!app.Environment.IsProduction())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<NewsDbContext>();
    dbContext.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();
