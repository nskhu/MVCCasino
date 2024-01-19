using BsMerchantAPI.Data.Repositories;
using BsMerchantAPI.Data.Repositories.Dapper;
using BsMerchantAPI.Services;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Config
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        cpbuilder => cpbuilder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
// Add Dapper
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

// add services and repos
builder.Services.AddTransient<IWalletRepository, WalletRepositoryDapper>();
builder.Services.AddTransient<IAuthRepository, AuthRepositoryDapper>();
builder.Services.AddTransient<IMerchantService, MerchantService>();
builder.Services.AddTransient<IAuthService, AuthService>();
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

app.UseCors("AllowAnyOrigin");

app.Run();