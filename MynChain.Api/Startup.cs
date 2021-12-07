using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.HealthChecks;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Api
{
    public class Startup
    {
        public static BlockChain chain;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //SWAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1.0",
                    Title = "MynChain API",
                    Description = "",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Myntech",
                        Email = "info@myntech.it",
                        Url = "http://www.myntech.it"
                    },
                    License = new License
                    {
                        Name = "License",
                        Url = "http://www.myntech.it"
                    }
                });
            });

            //HEALTH CHECK
            services.AddHealthChecks(checks =>
            {
                // First parameter is the URL you want to check
                // The second parameter is the CacheDuration of how long you want to hold onto the HealthCheckResult
                // The default is 5 minutes, and in my case, I don't really want any cache at all
                // because I don't want to wait up to 5 minutes if my service goes down to be notified about it
                //checks.AddCheck("https://github.com", new  TimeSpan.FromMilliseconds(1));
                //checks.AddUrlCheck("http://localhost:1314/");
                checks.AddValueTaskCheck("HTTP Endpoint", () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("OK")));
                checks.AddCheck("HC", new HC.HC());
            });

            //AUTHENTICATION
            //services.AddAuthentication(Configuration.GetValue<string>("Authentication:DefaultScheme"))
            //.AddJwtBearer(Configuration.GetValue<string>("Authentication:AuthenticationScheme"), options =>
            //{
            //    options.Authority = Configuration.GetValue<string>("Authentication:Authority");
            //    options.RequireHttpsMetadata = Configuration.GetValue<bool>("Authentication:RequireHttpsMetadata");
            //    options.Audience = Configuration.GetValue<string>("Authentication:Audience");
            //});

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            chain = new BlockChain();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //SWAGGER
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MynChain API v1.0");
            });

            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}
