using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Selah.WebAPI.Middleware;
using Selah.WebAPI.DependencyInjection.Extensions;
using Selah.Infrastructure;
using Selah.WebAPI.DependencyInjection;

namespace Selah.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly string _corsPolicy = "SelahCors";
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IDbConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(_config.GetValue<string>("DB_CONNECTION_STRING")));

            //Register JWT middleware
            services.ConfigureJwt(_config);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
            JwtBearerDefaults.AuthenticationScheme);

                defaultAuthorizationPolicyBuilder =
            defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
            services.RegisterHttpClients(_config);
            services.RegisterHashIds(_config);
            services.RegisterDbRepositories();
            services.RegisterApplicationServices();
            services.RegisterValidators();
            //Register Mediatr Commands/Queries
            services.RegisterQueries();
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Selah.WebAPI", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Selah.WebAPI v1"));
            }
            app.UseMiddleware<LoggerMiddleware>();
            app.UseRouting();
            app.UseCors(); // allow credentials
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}