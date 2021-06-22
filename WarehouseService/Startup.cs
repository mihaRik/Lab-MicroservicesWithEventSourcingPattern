using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseService.Database;
using WarehouseService.Services;

namespace WarehouseService
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
            //db context
            services.AddDbContext<WarehouseServiceDbContext>(options =>
            {
                var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            //swagger
            services.AddSwaggerGen(setup => setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Warehouse Service API", Version = "v1" }));

            //controllers
            services.AddControllers();

            //services
            services.AddScoped<Services.WarehouseService>();
            services.AddSingleton(container =>
            {
                var service = container.GetRequiredService<RabbitMQService>();

                service.GetJsonFromEvent("order_placed");

                return service;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(r =>
            {
                r.SwaggerEndpoint("/swagger/v1/swagger.json", "Warehouse Service API");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
