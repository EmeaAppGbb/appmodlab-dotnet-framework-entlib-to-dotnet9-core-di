using Serilog;
using Microsoft.EntityFrameworkCore;
using HealthClaimsProcessor.Core.Data;
using HealthClaimsProcessor.Core.DataAccess;
using HealthClaimsProcessor.Core.Interfaces;
using HealthClaimsProcessor.Core.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/healthclaims-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Health Claims Processor web application");

    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .WriteTo.Console()
        .WriteTo.File("logs/healthclaims-.txt", rollingInterval: RollingInterval.Day));

    // Add MVC services
    builder.Services.AddControllersWithViews();

    // Register DbContext
    builder.Services.AddDbContext<HealthClaimsDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Register repositories (interface → implementation)
    builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
    builder.Services.AddScoped<IPatientRepository, PatientRepository>();
    builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
    builder.Services.AddScoped<IProviderRepository, ProviderRepository>();

    // Register services (interface → implementation)
    builder.Services.AddScoped<IClaimService, ClaimService>();
    builder.Services.AddScoped<IPatientService, PatientService>();
    builder.Services.AddScoped<IPaymentService, PaymentService>();
    builder.Services.AddScoped<IProviderService, ProviderService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseStaticFiles();
    app.UseRouting();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Configure the static LoggingHelper bridge for backward compatibility
    HealthClaimsProcessor.Core.Logging.LoggingHelper.Configure(app.Services.GetRequiredService<ILoggerFactory>());

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
