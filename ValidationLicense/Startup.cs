using LicenseValidationApi;  // Add the namespace for DatabaseService
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace ValidationLicense
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var connection = Configuration.GetConnectionString("Dbconnection");
            var value = configuration.GetSection("Connection:Dbconnection").Value;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the DatabaseService as a Singleton
            services.AddSingleton<DatabaseService>();

            // Add the controllers (API controllers)
            services.AddControllers();

            // Add Swagger for API documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "License Validation API", Version = "v1" });

                // Optionally, you can add XML comments for more descriptive API documentation (if you have XML comments in your code)
                // var xmlFile = $"{AppDomain.CurrentDomain.FriendlyName}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // c.IncludeXmlComments(xmlPath);
            });

            // Add Database connection string configuration (Optional if you want to use the connection string in multiple places)
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Enable Swagger in development environment
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "License Validation API v1"));
            }

            // Redirect HTTP requests to HTTPS
            app.UseHttpsRedirection();

            // Use routing to map the incoming HTTP requests to controllers
            app.UseRouting();

            // Add authorization (if necessary) - Add authentication if required
            app.UseAuthorization();

            // Map controller routes to the HTTP request pipeline
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // This maps the controller actions to routes
            });
        }
    }
}
