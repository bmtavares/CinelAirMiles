namespace CinelAirMiles.Web.Backoffice
{
    using System;
    using System.Text;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;
    using CinelAirMiles.Common.Repositories.Classes;
    using CinelAirMiles.Web.Backoffice.Data;
    using CinelAirMiles.Web.Backoffice.Helpers.Classes;
    using CinelAirMiles.Web.Backoffice.Helpers.Interfaces;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Syncfusion.Licensing;

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
                cfg.UseSqlServer(Configuration.GetConnectionString("PublishConnection"), b => b.MigrationsAssembly("CinelAirMiles.Web.Backoffice"));
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
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IPartnerRepository, PartnerRepository>();
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
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/NotAuthorized";
            });

            services.AddMvc()
                .AddJsonOptions(options =>
                    options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            SyncfusionLicenseProvider
                .RegisterLicense("MzQ2OTU5QDMxMzgyZTMzMmUzMGlnZm82N0V2TUFTM2ZsSEJlRFZMZk1CSVVVZHlDN3VndUNwakhHczRNeWs9");

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

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/";
                    await next();
                }
            });

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
