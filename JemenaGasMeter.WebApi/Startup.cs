using System;
using System.IO;
using System.Reflection;
using JemenaGasMeter.WebApi.Data;
using JemenaGasMeter.WebApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AutoMapper;
using ServiceStack.Messaging;

namespace JemenaGasMeter.WebApi
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

            //Allow cors

            //add cors service
            services.AddCors(options => options.AddPolicy("Cors",
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                }));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString(nameof(AppDbContext)));
            });

            //Automapper
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Jemena API",
                    Description = "Jemena Meter Gas LifeCycle Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "RMIT",
                        Email = string.Empty,
                        Url = new Uri("https://rmit.edu.au"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Jemena",
                        Url = new Uri("https://jemena.com.au"),
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddTransient<DepotsRepository>();
            services.AddTransient<InstallationsRepository>();
            services.AddTransient<MetersRepository>();
            services.AddTransient<MeterHistoriesRepository>();
            services.AddTransient<UsersRepository>();
            services.AddTransient<WarehousesRepository>();
            services.AddTransient<TransfersRepository>();

            services.AddControllers();

            //  services.AddScoped<IMessageService, MessageService>();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jemena API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
