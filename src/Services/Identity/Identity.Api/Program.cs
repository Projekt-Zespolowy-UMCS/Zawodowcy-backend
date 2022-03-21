using Identity.Api.Configuration;
using Identity.Api.Extensions;
using Identity.Api.Utils;
using Identity.Infrastructure.Data;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.ConfigureServices();

builder.Services.AddControllers();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
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

app.UseCors(config => config
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();


app.MapControllers();

app.ConfigureDatabase();

app.Run();