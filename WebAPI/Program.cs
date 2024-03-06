using System.Net;
using Domain.AutoMapper;
using Microsoft.Extensions.Options;
using Serilog;
using Service;
using WebAPI.Settings;

// Add services to the container.

ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().MinimumLevel.Debug());

builder.Services.Configure<DaDataSettings>(builder.Configuration.GetSection("DaDataHeaders"));

builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("DaDataClient", (serviceProvider, client) =>
{
    var settings = serviceProvider
        .GetRequiredService<IOptions<DaDataSettings>>().Value;

    client.DefaultRequestHeaders.Clear();

    client.DefaultRequestHeaders.Add("Application-type", settings.ApplicationType);
    client.DefaultRequestHeaders.Add("Authorization", "Token " + settings.Token);
    client.DefaultRequestHeaders.Add("X-Secret", settings.Secret);

    client.BaseAddress = new Uri(settings.BaseUri);
});

builder.Services.AddTransient<IDaDataService, DaDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
