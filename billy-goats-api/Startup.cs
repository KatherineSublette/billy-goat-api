using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BillyGoats.Api.Models.DBContext;
using BillyGoats.Api.Utils.Extensions;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Data.Repositories;

namespace BillyGoats.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Add Database DbContext.
            var connString = Configuration.GetDbConnectionString(typeof(BillyGoatsDb).Name);
            services.AddDbContext<BillyGoatsDb>(options =>
            {
                options.UseNpgsql(connString);
            });

            // add cross-domain policies to allow cross-domain access during development
            if (this.Configuration["Cors:AllowedOrigins"] != null)
            {
                services.AddCors(o => o.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins(this.Configuration["Cors:AllowedOrigins"]);
                }));
            }

            // injecting DBContext
            services.AddScoped<DbContext, BillyGoatsDb>();

            // injecting Generic Repository for the rest of the Data Models
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

            // injecting Generic data service
            services.AddScoped(typeof(IDataService<>), typeof(DataService<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMvc();

            if (this.Configuration["Cors:AllowedOrigins"] != null)
            {
                app.UseCors("CorsPolicy");
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
