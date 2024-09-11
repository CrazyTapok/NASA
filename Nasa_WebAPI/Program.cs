using Microsoft.EntityFrameworkCore;
using NAS_BAL.EF;
using Nasa_BAL.Interfaces;
using Nasa_BAL.Jobs;
using Nasa_BAL.Services;
using Nasa_WebAPI.Extensions.Configuration;
using Quartz;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File("LogsApp/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddServices();

builder.Services.AddHttpClient();

builder.Services.AddQuartz(t =>
{
    var jobKey = new JobKey("GetMeteoritesJob");

    t.AddJob<GetMeteoritesJob>(opts => opts.WithIdentity(jobKey));

    t.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("Immediate-GetMeteoritesJob-trigger")
        .StartNow());

    t.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("Scheduled-GetMeteoritesJob-trigger")
        .WithCronSchedule("0 0/30 * * * ?"));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
