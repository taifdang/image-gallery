using Application;
using Infrastructure.Messaging;
using Persistence;
using WebAPI.ConfigurationOptions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

builder.AddServiceDefaults();

services.AddOpenApi();

services.AddControllers();

services.AddPersistence(appSettings.ConnectionStrings.DefaultConnection)
        .AddApplicationServices()
        .AddMessaging(appSettings.Messaging);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.MapControllers();

app.Run();