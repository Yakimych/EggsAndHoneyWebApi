using System;
using EggsAndHoney.Domain.Models;
using EggsAndHoney.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;

namespace EggsAndHoney.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddAutoMapper();
            RegisterOrderService(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Eggs&Honey Web API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eggs&Honey Web API V1");
            });
        }

        private void RegisterOrderService(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection("AppSettings");
            var useInMemoryServiceSetting = appSettings["UseInMemoryConfiguration"];

            services.AddTransient<IOrderService, OrderService>();

            if (bool.TryParse(useInMemoryServiceSetting, out var useInMemoryService) && useInMemoryService)
            {
                var inMemoryDbName = Guid.NewGuid().ToString();
                services.AddDbContext<OrderContext>(options => options.UseInMemoryDatabase(inMemoryDbName));

                // Is there a better way to do this than fetching the DbContext from the IoC container?
                // One option would be to actually use DbInitializer no matter what
                var serviceProvider = services.BuildServiceProvider();
                var context = serviceProvider.GetService<OrderContext>();
                var orderTypeSet = context.Set<OrderType>();
                orderTypeSet.Add(new OrderType { Id = 1, Name = "Eggs" });
                orderTypeSet.Add(new OrderType { Id = 2, Name = "Honey" });
                context.SaveChanges();
            }
            else
            {
                var connectionString = Configuration.GetConnectionString("EggsAndHoneyConnectionString");
                services.AddDbContext<OrderContext>(options => options.UseSqlServer(connectionString));
            }
        }
    }
}
