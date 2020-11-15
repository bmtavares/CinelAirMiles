namespace CinelAirMiles.Web.InternalAPI
{
    using System;

    using CinelAirMiles.Common.Data;
    using CinelAirMiles.Common.Entities;
    using CinelAirMiles.Common.Repositories;
    using CinelAirMiles.Common.Repositories.Classes;
    using CinelAirMiles.Common.Services;
    using CinelAirMiles.Web.InternalAPI.Helpers;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
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

            services.AddDbContext<ApplicationDbContext>(cfg =>
            {
                if (_env.IsDevelopment())
                {
                    cfg.UseSqlServer(Configuration.GetConnectionString("DevConnection"), b => b.MigrationsAssembly("CinelAirMiles.Web.Backoffice"));
                }
                else
                {
                    cfg.UseSqlServer(Configuration.GetConnectionString("ProdConnection"));
                }
            });

            services.AddScoped<IMathHelper, MathHelper>();
            services.AddScoped<IMilesHelper, MilesHelper>();
            services.AddScoped<IApiService, ApiService>();

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IMileRepository, MileRepository>();
            services.AddScoped<IProgramTierRepository, ProgramTierRepository>();

            services.AddScoped<ISeatClassRepository, SeatClassRepository>();

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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
