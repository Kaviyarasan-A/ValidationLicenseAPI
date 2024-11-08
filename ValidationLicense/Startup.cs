using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using LicenseValidationApi;  // Add the namespace for DatabaseService

namespace ValidationLicense
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            var connection = Configuration.GetConnectionString("Dbconnection");
            var value = configuration.GetSection("Connection:Dbconnection").Value;
            Configuration = configuration;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure CORS (Allow all origins, methods, and headers)
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Register the IDbConnection with the connection string from appsettings.json
            services.AddSingleton<IDbConnection>(sp =>
            {
                var connectionString = Configuration.GetConnectionString("Dbconnection");
                return new SqlConnection(connectionString);
            });

            // Register the DatabaseService as a Singleton
            services.AddSingleton<DatabaseService>();

            // Add other services
            services.AddControllers();

            // Add Swagger for API documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "License Validation API", Version = "v1" });
            });

            // Add the configuration for later access (optional)
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll"); // Enable CORS

            // Other middleware (Swagger, Routing, etc.)
            app.UseRouting();

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

            // Add authorization middleware if necessary
            app.UseAuthorization();

            // Map controller routes to the HTTP request pipeline
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // This maps the controller actions to routes
            });
        }
    }
}
