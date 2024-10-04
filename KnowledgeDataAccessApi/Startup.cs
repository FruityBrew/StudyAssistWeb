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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
