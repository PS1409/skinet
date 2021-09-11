using API.Extensions;
using API.Helpers;
using API.Middleware;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            services.AddDbContext<StoreContext>(opt =>
                     opt.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(x =>
            {
                x.UseSqlite(_config.GetConnectionString("IdentityConnection"));
            });
            services.AddSingleton<IConnectionMultiplexer>(c =>
           {
               var connection = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
               return ConnectionMultiplexer.Connect(connection);
           });

            services.AddApplicationServices();
            services.AddIdentityService(_config);
            
            services.AddSwaggerDocumentation();
            services.AddCors(Opt =>
            {
                Opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerDocumentation();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
