using Hellang.Middleware.ProblemDetails;
using HotelFinal.Server;
using HotelFinal.Shared;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using System.Diagnostics;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole();
builder.Logging.AddDebug(); 

builder.Logging.AddSimpleConsole();
/*builder.Services.AddHealthChecks()
    .AddDbContextCheck<HotelContext>();*/
// builder.Services.AddApplicationInsightsTelemetry();

builder.Host.UseSerilog((context, LoggerConfig) =>
{
    LoggerConfig.ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console()
    .Enrich.WithExceptionDetails()
    .Enrich.FromLogContext()
    .Enrich.With<ActivityEnricher>()
    .WriteTo.Seq("http://localhost:5321");
});

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.WriteTo.Console().WriteTo.Seq("https://localhost:5324");

});

//NLog.LogManager.Setup().LoadConfigurationFromFile;
builder.Host.UseNLog();

builder.Services.AddProblemDetails(opts =>
{
    opts.IncludeExceptionDetails = (ctx, ex) => false;
    opts.OnBeforeWriteDetails = (ctx, dtls) => { 
        if(dtls.Status == 500)
        {
            dtls.Detail = "an error occured in the api or server";
        }
    };
}
);

// Add services to the container.

var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var tracePath = Path.Join(path, $"Log_RentalControllor{DateTime.Now.ToString("yyyyMMdd-HHmm")}.txt");
Trace.Listeners.Add(new TextWriterTraceListener(System.IO.File.CreateText(tracePath)));
Trace.AutoFlush= true;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
    {
        c.Authority = $"{builder.Configuration["Auth0:Domain"]}";
        c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
        };
    });

var conStr = builder.Configuration.GetConnectionString("pg");
builder.Services.AddDbContext<HotelContext>(options => options.UseNpgsql(conStr));

builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseProblemDetails();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapHealthChecks("health").AllowAnonymous();

app.Run();
