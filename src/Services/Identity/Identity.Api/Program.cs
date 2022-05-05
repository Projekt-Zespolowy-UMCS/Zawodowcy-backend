using Autofac.Extensions.DependencyInjection;
using Identity.Api.Configuration;
using Identity.Api.Extensions;
using Identity.Api.Utils;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(options =>
{
    var port = System.Environment.GetEnvironmentVariable("PORT");
    if (!string.IsNullOrWhiteSpace(port))
        options.ListenAnyIP(Int32.Parse(port));
});
// Add services to the container.
builder.ConfigureServices();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

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

app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
{
    builder.UseSpaStaticFiles();
    builder.UseSpa(spa =>
    {
        if (app.Environment.IsDevelopment())
        {
            spa.Options.SourcePath = "client-ui";
            spa.UseReactDevelopmentServer("start");
        }
    });
});

app.Run();
