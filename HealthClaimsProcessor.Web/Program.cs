using Serilog;
using HealthClaimsProcessor.Core.DataAccess;
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

    // Register repositories (scoped)
    builder.Services.AddScoped<ClaimRepository>();
    builder.Services.AddScoped<PatientRepository>();
    builder.Services.AddScoped<PaymentRepository>();
    builder.Services.AddScoped<ProviderRepository>();

    // Register services (scoped)
    builder.Services.AddScoped<ClaimService>();
    builder.Services.AddScoped<PatientService>();
    builder.Services.AddScoped<PaymentService>();
    builder.Services.AddScoped<ProviderService>();

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
