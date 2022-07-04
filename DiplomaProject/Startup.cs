using System;
using System.Text;
using DiplomaProject.IRepositories;
using DiplomaProject.Models;
using DiplomaProject.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiplomaProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddRazorPages().AddMvcOptions(options =>
            {
                options.MaxModelValidationErrors = 50;
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    _ => "Це поле має бути заповнене.");
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
                     () => "Це поле має бути заповнене.");
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
                    _ => "Некоректний формат даних");
            });

            services.AddControllers();
            
            var environmentName = Environment.GetEnvironmentVariable("Env");

            var connString = "";

            var dbUrl = Environment.GetEnvironmentVariable("DB_URL");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
            
            if (dbUrl != null && dbUser != null && dbPass != null)
            {
                var sb = new StringBuilder();
                
                sb.Append($"server={dbUrl};port=3306;user={dbUser};password={dbPass};database=CraftDatabase");

                connString = sb.ToString();
            }
            else
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{environmentName}.json", true)
                    .AddEnvironmentVariables()
                    .Build();
            
                // other service configurations go here
                connString = configuration.GetConnectionString("DefaultConnection");   
            }

            services.AddDbContext<KraftWebAppContext>(options => options.UseMySql(connString, 
                            ServerVersion.AutoDetect(connString)));
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IShopProfileRepository, ShopProfileRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = ".DiplomaProject.MyCart.Session";
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Domain = ".handmade-space.top";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    // автентификація
            app.UseAuthorization();     // авторизація

            app.UseSession();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}