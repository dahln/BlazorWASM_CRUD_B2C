using BlazorWASM_CRUD_B2C.Data;
using BlazorWASM_CRUD_B2C.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

//Build App
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlite($"Data Source=dotnetBUILT_BlazorWASM_CRUD_B2C.db"));
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    // ^^^ use this option for MSSQL instead of Sqlite ^^^
    // If MSSQL is used, remove 'UseSqlite'

builder.Services.AddControllers();

builder.Services.AddScoped<CustomerService>();

const string corsName = "CORSPolicy";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: corsName, policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors").Value)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


//Config App
var app = builder.Build();

//Check for and automatically apply pending migrations
var db = app.Services.GetService<DbContext>();
if (db != null)
{
    var migrations = db.Database.GetPendingMigrations();
    if (migrations.Count() > 0)
        db.Database.Migrate();
}

app.UseCors(corsName);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
