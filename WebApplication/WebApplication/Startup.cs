namespace WebApplication
{
    using Autofac;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using WebApplication.Configuration;
    using WebApplication.Core;

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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "shortUri",
                    template: "{id}",
                    defaults: new { controller = "Redirect", action = "IndexAsync" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Configure the container builder.
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/></param>
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            var apiOptions = this.Configuration.GetSection("Api").Get<ApiOptions>();

            builder.Register(p => apiOptions)
                .SingleInstance();

            builder.RegisterInstance(Options.Create(apiOptions));

            builder.Register(p => this.Configuration.GetSection("AzureTable").Get<AzureTableStorageOptions>())
                .SingleInstance();

            builder.RegisterType<ShortenUrlGenerator>()
                .As<IShortenUrlGenerator>()
                .SingleInstance();

            builder.RegisterType<AzureTableRepository>()
                .As<IRepository>()
                .SingleInstance();
        }
    }
}