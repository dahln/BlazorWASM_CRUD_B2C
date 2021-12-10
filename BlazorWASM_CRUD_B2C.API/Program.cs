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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    //options.UseSqlite($"Data Source=BlazorWASM_CRUD_B2C.db"));

builder.Services.AddControllers();

builder.Services.AddScoped<CustomerService>();

const string corsName = "CORSPolicy";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: corsName, policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors").Value.Split(","))
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


//Config App
var app = builder.Build();

//Check for and automatically apply pending migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<DBContext>();
    if (db != null)
    {
        var migrations = db.Database.GetPendingMigrations();
        if (migrations.Any())
            db.Database.Migrate();
    }
}

app.UseCors(corsName);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
