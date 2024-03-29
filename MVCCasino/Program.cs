using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVCCasino.Data;
using MVCCasino.Data.Repository;
using MVCCasino.Data.Repository.Dapper;
using MVCCasino.Models;
using MVCCasino.Services;
using MVCCasino.Settings;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddTransient<IDbConnection>(_ => new SqlConnection(connectionString));
builder.Services.AddTransient<IWalletRepository, WalletRepositoryDapper>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepositoryDapper>();
builder.Services.AddTransient<IAuthRepository, AuthRepositoryDapper>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.Configure<BankApiSettings>(builder.Configuration.GetSection("BankApiSettings"));
builder.Services.Configure<MerchantApiSettings>(builder.Configuration.GetSection("MerchantApiSettings"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBankOrigin",
        cpbuilder => cpbuilder.WithOrigins("http://localhost:5264")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        cpbuilder => cpbuilder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddLogging(logBuilder =>
{
    logBuilder.AddConsole();
    logBuilder.SetMinimumLevel(LogLevel.Trace);
});
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
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
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowBankOrigin");
app.UseCors("AllowAnyOrigin");
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();