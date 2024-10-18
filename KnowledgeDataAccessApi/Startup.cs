using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using KnowledgeDataAccessApi.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using KnowledgeDataAccessApi.Validators;
using Microsoft.Extensions.Configuration;
using IdentityServer4.AccessTokenValidation;
namespace KnowledgeDataAccessApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private IWebHostEnvironment _webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _webHostEnvironment = environment;

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = this.Configuration.GetConnectionString("RemoteKnowledgeConnection");
            services.AddDbContext<KnowledgeContext>(
                opt => opt.UseSqlServer(connectionString));

            services.AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = 
                        ReferenceLoopHandling.Ignore;
                });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetValue<string>("AuthConfig:IdentityServerAuthorityUrl");
                    options.Audience = Configuration.GetValue<string>("AuthConfig:IdentityServerResourcesUrl"); ;
                    options.RequireHttpsMetadata = Configuration.GetValue<string>("AuthConfig:IdentityServerAuthorityUrl").Contains("https");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("KnowledgeApi.Read", policyUser =>
                {
                    policyUser.RequireClaim("scope", "Author", "User");
                });

				options.AddPolicy("KnowledgeApi.Write", policyUser =>
				{
					policyUser.RequireClaim("scope", "Author");
				});
			});

            services.AddHttpClient();
            services.AddMvc()
                .AddFluentValidation(fvConfig =>
                    fvConfig.RegisterValidatorsFromAssemblyContaining<CatalogValidator>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
