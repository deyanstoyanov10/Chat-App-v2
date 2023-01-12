using AspNetCore.SEOHelper;
using ChatApp.Host.Common;
using ChatApp.Host.Configuration;
using ChatApp.Host.Data;
using ChatApp.Host.Producer.Implementations;
using ChatApp.Host.Producer.Interfaces;
using ChatApp.Host.Services.Implementations;
using ChatApp.Host.Services.Interfaces;
using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services
    .Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)))
    .Configure<MessageLoggingConfig>(builder.Configuration.GetSection(nameof(MessageLoggingConfig)));

builder.Services
    .AddSingleton(typeof(ISerializer<>), typeof(Serializer<>))
    .AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>))
    .AddSingleton<IIdProvider, IdProvider>()
    .AddSingleton<IDateTimeProvider, DateTimeProvider>()
    .AddSingleton<IMessageService, MessageService>();


builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseXMLSitemap(app.Environment.ContentRootPath);
app.UseRobotsTxt(app.Environment.ContentRootPath);
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x
         .AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());

using var services = app.Services.CreateScope();
var dbContext = services.ServiceProvider.GetService<ApplicationDbContext>();

dbContext.Database.Migrate();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "api",
    pattern: "api/v{version:apiVersion}/{controller}/{action}");
app.MapRazorPages();

app.Run();
