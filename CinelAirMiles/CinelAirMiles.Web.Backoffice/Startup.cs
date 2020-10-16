using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinelAirMiles.Web.Backoffice.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CinelAirMiles.Web.Backoffice.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CinelAirMiles.Web.Backoffice.Data.Repositories.Classes;
using CinelAirMiles.Web.Backoffice.Data.Repositories.Interfaces;
using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;
using CinelAirMiles.Web.Backoffice.Helpers.Classes;

namespace CinelAirMiles.Web.Backoffice
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

            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                cfg.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 30, 0);
                cfg.SignIn.RequireConfirmedEmail = true;
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = true;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = true;
                cfg.Password.RequireNonAlphanumeric = true;
                cfg.Password.RequireUppercase = true;
                cfg.Password.RequiredLength = 8;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };
                });


            services.AddDbContext<ApplicationDbContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("PublishConnection"));
            });


            services.AddTransient<Seed>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ICreditCardRepository, CreditCardRepository>();
            services.AddScoped<IMileRepository, MileRepository>();
            services.AddScoped<IMilesTransactionRepository, MilesTransactionRepository>();
            services.AddScoped<IMilesTypeRepository, MilesTypeRepository>();
            services.AddScoped<IProgramTierRepository, ProgramTierRepository>();
            services.AddScoped<IMilesTypeRepository, MilesTypeRepository>();
            services.AddScoped<IProgramTierRepository, ProgramTierRepository>();
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<ICombosHelper, CombosHelper>();
            services.AddScoped<IConverterHelper, ConverterHelper>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IMailHelper, MailHelper>();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/account/notauthorized";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
