using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtSampleService.Services;
using JwtSampleService.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JwtSampleService
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
            services.AddMvc();

            var jwtSection = Configuration.GetSection("jwt");
            var jwtSettings = new JwtSettings();
            jwtSection.Bind(jwtSettings);

            var jwtAuthService = new JwtAuthService(jwtSettings);

            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = jwtAuthService.Parameters;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
