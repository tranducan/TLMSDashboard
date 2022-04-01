using EFTechlink.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLMSData.Interfacing;
using TLMSData.Processing;

namespace TLMSDashboard
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
            services.AddControllersWithViews();
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TLMSDataContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);
            services.AddSingleton<GetPQCData>();
            services.AddSingleton<GetPQCDataSummary>();
            services.AddTransient<TLMSDataContext>();
            services.AddScoped<ISetDailyTarget, SetDailyTarget>();
            services.AddScoped<IGetPQCData, GetPQCData>();
            services.AddScoped<IMqcDataInteracting, MqcDataInteracting>();
            services.AddScoped<IGetPQCDataSummary, GetPQCDataSummary>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                Parameters.SetStaticValue.isDevEnvironment = true;
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                Parameters.SetStaticValue.isDevEnvironment = false;
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
