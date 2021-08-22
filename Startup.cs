using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetsRegApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetsRegApi.Services.PetService;

namespace PetsRegApi
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
            // for Azure AD Authentication
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = AzureADDefaults.AuthenticationScheme;
            })
                .AddJwtBearer("AzureAD", options =>
                {
                    options.Audience = Configuration.GetValue<string>("AzureAd:Audience");
                    options.Authority = Configuration.GetValue<string>("AzureAd:Instance") 
                    + Configuration.GetValue<string>("AzureAd:TenantId");

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidIssuer = Configuration.GetValue<string>("AzureAd:Issuer"),
                        ValidAudience = Configuration.GetValue<string>("AzureAd:Audience")
                    };
                });
            // end Azure AD Authentication

            services.AddControllers();

            services.AddDbContext<PetsDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DevDB")));

            /*services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration, "AzureAd");*/

            // for Service
            services.AddScoped<IPetService, PetService>();

            // for CORS error
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // for CORS error (must be at the first line of the code)
            app.UseCors(options =>
            /*//options.WithOrigins("http://localhost:3000")*/
            options.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod());
            // end for CORS error

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication(); //AD Authentication

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
