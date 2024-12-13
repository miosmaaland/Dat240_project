using FluentValidation;
using MediatR;
using System.Reflection;
using SmaHauJenHoaVij.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Domain.Users.Services;
using SmaHauJenHoaVij.Infrastructure.Services;
using SmaHauJenHoaVij.Core.Domain.Users.Pipelines;
using SmaHauJenHoaVij.Infrastructure.Middleware;
using SmaHauJenHoaVij.Core.Domain.Ordering.Services;
using Core.Domain.Fulfillment.Pipelines;
using SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Handlers;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
using SmaHauJenHoaVij.Core.Domain.Users.Events;
using SmaHauJenHoaVij.Core.Domain.Users.Handlers;
using SmaHauJenHoaVij.Infrastructure;
using Stripe;
using Microsoft.AspNetCore.Builder;
using SmaHauJenHoaVij.Infrastructure.Payments.Stripe;
using SmaHauJenHoaVij.Infrastructure.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen only on HTTP
builder.Services.AddSignalR();
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5268);  // Only HTTP
});

// Stripe configuration
var stripeSection = builder.Configuration.GetSection("Stripe");
builder.Services.Configure<StripeOptions>(stripeSection);
StripeConfiguration.ApiKey = stripeSection["SecretKey"];

builder.Services.Configure<StripeOptions>(stripeSection);
builder.Services.AddTransient<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddSingleton<NotificationService>();

builder.Services.AddScoped<ExternalLoginHandler>();
builder.Services.AddScoped<ExternalLoginCallbackHandler>();

// Session and caching setup
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
});

// Register MediatR and other services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(UserRegistered).Assembly,
    typeof(CourierApplicationApprovedHandler).Assembly,
    typeof(CourierApplicationDeclinedHandler).Assembly,
    typeof(OrderPickedUp).Assembly,
    typeof(OrderDelivered).Assembly,
    typeof(Program).Assembly
));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<DeliveryFeeService>();

// Authentication configuration
builder.Services.AddAuthentication(options =>
{
    // Use cookies for authentication by default
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Login"; // Redirect to login if not authenticated
    options.LogoutPath = "/Logout"; // Redirect to logout page
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    options.Scope.Add("email");
    options.Fields.Add("email");
    options.Fields.Add("name");
    options.SaveTokens = true;
});

builder.Services.AddSignalR();

// DB Context configuration
builder.Services.AddDbContext<ShopContext>(options =>
{
    options.UseSqlite($"Data Source={Path.Combine("Infrastructure", "Data", "shop.db")}");
});

builder.Services.Scan(scan => scan
    .FromAssemblyOf<ShopContext>()
    .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
    .AsImplementedInterfaces());

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<UpdateCustomerProfilePipeline>();
builder.Services.AddScoped<IOrderingService, OrderingService>();

// Add Razor Pages and authorization
builder.Services.AddRazorPages();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("UserType", "Admin"));
    options.AddPolicy("CourierOnly", policy => policy.RequireClaim("UserType", "Courier"));
    options.AddPolicy("CustomerOnly", policy => policy.RequireClaim("UserType", "Customer"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // Get the database context
        var context = services.GetRequiredService<ShopContext>();

        // Apply migrations (optional)
        context.Database.Migrate();

        // Seed data
        SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        // Log any errors that occur during the seeding process
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();  // This might still enforce HTTPS
}

app.MapControllers();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseMiddleware<AdminPasswordResetMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

// Configure Razor Pages and routing
app.MapRazorPages();

app.UseExternalAuthenticationPipeline();

Console.WriteLine("Application started on http://localhost:5268");

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
