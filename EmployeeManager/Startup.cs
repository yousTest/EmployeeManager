using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeManager
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDataBaseConnection")));

            services.AddMvc().AddXmlSerializerFormatters();
            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                //default exception handler middelware
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();
            //app.UseMvc(route =>
            //{
            //    route.MapRoute("default", "{controller=Home}/{action=Index}/{Id?}");
            //});

            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!  " + _config["myKey"]);
            //});
        }
    }
}
