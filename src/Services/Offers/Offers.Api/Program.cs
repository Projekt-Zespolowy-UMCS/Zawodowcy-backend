using Microsoft.OpenApi.Models;
using Offers.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureServices();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() { Title = "Offers.Api", Version = "v1" }); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Offers.Api v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ConfigureDatabase();

app.Run();