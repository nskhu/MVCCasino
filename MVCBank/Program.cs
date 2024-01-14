using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCBank.Data;
using MVCBank.Services;
using MVCBank.Settings;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.Configure<BankApiSettings>(builder.Configuration.GetSection("BankApiSettings"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCasinoOrigin",
        cpbuilder => cpbuilder.WithOrigins("http://localhost:5163")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddLogging(logBuilder =>
{
    logBuilder.AddConsole();
    logBuilder.SetMinimumLevel(LogLevel.Trace);
});
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
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
app.UseRouting();
app.UseAuthorization();
app.UseCors("AllowCasinoOrigin");
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();