﻿// TODO set config from appsettings.json

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Configuration.AddEnvironmentVariables("News_");

builder.Services
    .AddNewsOptions(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddCors()
    .AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "News",
        Version = "v1",
        Description = "API providing interface for operations related to news",
        Contact = new OpenApiContact { Url = new Uri("https://github.com/PetrKoller") },
    }))
    .AddScoped<NewsDbContext>()
    .AddScoped<INewsDbContext, NewsDbContext>()
    .AddMediator(o =>
    {
        o.Namespace = "Politicz.News.Mediator";
        o.ServiceLifetime = ServiceLifetime.Transient;
    })
    .AddScoped<IPipelineBehavior<CreateNewsCommand, OneOf<NewsEntity, Failure>>, ValidationBehavior<CreateNewsCommand, NewsEntity>>()
    .AddScoped<IPipelineBehavior<UpdateNewsCommand, OneOf<NewsEntity, NotFound, Failure>>, ValidationBehavior<UpdateNewsCommand, NewsEntity>>()
    .AddValidatorsFromAssemblyContaining<CreateNewsValidator>();

var app = builder.Build();
app.UseSerilogRequestLogging();

// TODO Global error handler
app.UseCors(c => c
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.MapNews();

if (!app.Environment.IsProduction())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<NewsDbContext>();
    dbContext.Database.Migrate();
}

// app.UseHttpsRedirection();

// app.UseAuthentication();
// app.UseAuthorization();
app.Run();
