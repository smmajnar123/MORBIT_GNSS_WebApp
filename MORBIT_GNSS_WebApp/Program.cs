using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MORBIT_GNSS_APP.Core.Events;
using MORBIT_GNSS_APP.Core.Interface;
using MORBIT_GNSS_APP.Core.Models;
using MORBIT_GNSS_APP.DataAccessLayer;
using MORBIT_GNSS_APP.DataAccessLayer.Models;
using MORBIT_GNSS_APP.Repository.IRepository;
using MORBIT_GNSS_APP.Repository.Repository;
using MORBIT_GNSS_APP.Service.IService;
using MORBIT_GNSS_APP.Service.Models;
using MORBIT_GNSS_APP.Service.Parse;
using MORBIT_GNSS_APP.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddDbContext<MorbitGnssAppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MongoDB Settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Mongo Client
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = builder.Configuration
        .GetSection("MongoDbSettings")
        .Get<MongoDbSettings>();

    return new MongoClient(settings?.ConnectionString);
});

builder.Services.AddScoped<IGnssDataRepository, GnssDataRepository>();

//core
builder.Services.AddSingleton<ISerialGnssServiceModel, SerialGnssServiceModel>();
builder.Services.AddSingleton<INmeaLogParseServiceModel, NmeaLogParseServiceModel>();
builder.Services.AddSingleton<IGnssEvent, GnssEvent>();
builder.Services.AddSingleton<INmeaEvent, NmeaEvent>();

//service
builder.Services.AddSingleton<ISerialGnssService, SerialGnssService>();
builder.Services.AddSingleton<INmeaLogParseService, NmeaLogParseService>();
builder.Services.AddSingleton<INmeaBaseModel, NmeaBaseModel>();
builder.Services.AddTransient<ILogParseFactory, LogParseFactory>();
builder.Services.AddSingleton<ILogParse, GgaLogParse>();
builder.Services.AddSingleton<ILogParse, GsaLogParse>();
builder.Services.AddSingleton<ILogParse, RmcLogParse>();
builder.Services.AddSingleton<ILogParse, GsvLogParse>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ✅ Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // ✅ must be before MapControllers()

app.UseAuthorization();

app.MapControllers();

app.Run();
