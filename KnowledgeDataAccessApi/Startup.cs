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
namespace KnowledgeDataAccessApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = @"Server=KOVALEVAS\SQLEXPRESS;Database=KnowledgeDb;Trusted_Connection=true;";
            //string connectionString = @"workstation id=knowledgeDb.mssql.somee.com;packet size=4096;user id=FruityBrew_SQLLogin_1;pwd=4f65smahle;data source=knowledgeDb.mssql.somee.com;persist security info=False;initial catalog=knowledgeDb";
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
                    fvConfig.RegisterValidatorsFromAssemblyContaining<CatalogValidator>())
                .AddFluentValidation(fvConfig =>
                    fvConfig.RegisterValidatorsFromAssemblyContaining<CatalogUpdatePatchValidator>())
                .AddFluentValidation(fvConfig =>
                    fvConfig.RegisterValidatorsFromAssemblyContaining<ThemeValidator>())
                .AddFluentValidation(fvConfig =>
                    fvConfig.RegisterValidatorsFromAssemblyContaining<ThemeUpdatePatchValidator>())
                .AddFluentValidation(fvConfig =>
                    fvConfig.RegisterValidatorsFromAssemblyContaining<IssueValidator>())
                .AddFluentValidation(fvConfig =>
                    fvConfig.RegisterValidatorsFromAssemblyContaining<IssueUpdatePatchValidator>());
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
