using IdentityNumber.Domain.Repositories;
using IdentityNumber.Domain.Services;
using IdentityNumber.Infrastructure.Api.Controllers;
using IdentityNumber.Infrastructure.Repositories;
using IdentityNumber.Infrastructure.Repositories.Csv;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityNumber.Infrastructure.Api
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
            //services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<ILogger, Logger<IdentityNumberController>>();
            services.AddScoped<IValidIdentityNumberRepository, ValidIdentityNumberCsvRepository>();
            services.AddScoped<IInvalidIdentityNumberRepository, InvalidIdentityNumberCsvRepository>();
            services.AddScoped<IIdentityNumberService, IdentityNumberService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(
                options => options
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
