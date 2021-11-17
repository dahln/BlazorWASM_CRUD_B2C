using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Security.Claims;
using BlazorWASM_CRUD_B2C.API.Utility;
using BlazorWASM_CRUD_B2C.Data;
using BlazorWASM_CRUD_B2C.Service;

namespace BlazorWASM_CRUD_B2C.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private string corsName = "CORSPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));

            services.AddDbContext<DBContext>(options =>
                options.UseSqlite($"Data Source=dotnetBUILT_BlazorWASM_CRUD_B2C.db"));

            /* 
            Preference is to use MSSQL. SQLite is used for demo purposes.
            If you deploy this application with
            If MSSQL replaces SQLite - Migrations will not be compatible, delete migrations and start over from that point.
            Also, if MSSQL is used, include a connecting string appsettings.
            */
            //services.AddDbContext<DBContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<CustomerService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlazorWASM_CRUD_B2C.API", Version = "v1" });
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy(name: corsName, builder =>
                {
                    builder.WithOrigins(Configuration.GetSection("Cors").Value)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DBContext dbContext)
        {
            //Automatic DB migrations on startup.
            //Remove this if you want to update the DB another way
            var migrations = dbContext.Database.GetPendingMigrations();
            if (migrations.Count() > 0)
                dbContext.Database.Migrate();

            app.UseCors(corsName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlazorWASM_CRUD_B2C.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
