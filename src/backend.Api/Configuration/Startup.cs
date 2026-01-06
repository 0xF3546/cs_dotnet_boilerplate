using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using backend.DataAccess;
using System.Text.Json.Serialization;

namespace backend.Api.Configuration
{
    public class Startup(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly Logger<Startup> _logger;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddOpenApiDocument(c =>
                c.PostProcess = document =>
                {
                    document.Info = new NSwag.OpenApiInfo
                    {
                        Title = "Backend API",
                        Version = "v1",
                        Description = "Backend",
                        Contact = new NSwag.OpenApiContact
                        {
                            Name = "Backend",
                            Email = ""
                        }
                    };
                }
            );

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });


            services.AddCors(c =>
            {
                c.AddDefaultPolicy(new CorsPolicyBuilder().WithOrigins(_configuration.GetSection("CorsUrls").Get<string[]>())
                    .AllowAnyMethod()
                .AllowAnyHeader()
                    .AllowCredentials()
                    .Build());
            });

            services.ConfigureCoreServices();
            services.ConfigureDataAccess();
            services.ConfigureDatabase(_configuration);

            services.AddAuthorization();
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;

                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}