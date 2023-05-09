using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IVC.PE.DATA.Context;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Seed;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Http;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Http.Features;
using IVC.PE.WEB.Factories;
using ServiceWorkerCronJobDemo.Jobs;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//using IVC.PE.WEB.Filters;

namespace IVC.PE.WEB
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
            //// Register the Swagger generator, defining 1 or more Swagger documents
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ivc API", Version = "v1", TermsOfService = new Uri("https://example.com/terms")});
            //    c.OperationFilter<AddAuthHeaderOperationFilter>();
            //    c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            //    {
            //        Description = "`Token only!!!` - without `Bearer_` prefix",
            //        Type = SecuritySchemeType.Http,
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Scheme = "bearer"
            //    });
            //});

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            services.Configure<FormOptions>(o =>  // currently all set to max, configure it to your needs!
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = long.MaxValue; // <-- !!! long.MaxValue
                o.MultipartBoundaryLengthLimit = int.MaxValue;
                o.MultipartHeadersCountLimit = int.MaxValue;
                o.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddDbContext<IvcDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"), 
                    sqlOpts => sqlOpts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)),
                    ServiceLifetime.Transient);
            
            services.AddIdentity<ApplicationUser, ApplicationRole>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
                .AddEntityFrameworkStores<IvcDbContext>()
                .AddDefaultTokenProviders();

            // Add hosted services
            services.AddHostedService<PayrollQueuedBackground>();
            services.AddTransient<IPayrollQueuedBackground, PayrollQueuedBackground>();
            services.AddHostedService<RacsQueuedBackground>();
            services.AddTransient<IRacsQueuedBackground, RacsQueuedBackground>();
            services.AddHostedService<EmailQueuedBackground>();
            services.AddTransient<IEmailQueuedBackground, EmailQueuedBackground>();

            // Add application services.
            services.AddSingleton<IEmailSender, EmailSender>();            

            services.Configure<CloudStorageCredentials>(Configuration.GetSection("AzureStorageCredentials"));
            services.AddTransient<ICloudStorageService, CloudStorageService>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ClaimsPrincipalFactory>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddSessionStateTempDataProvider();  // Solve => Error 400 Headers too long for TempData

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };
                });

            services.AddSession();

            services.AddCronJob<MyCronJob1>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"15 8,20 * * *";
                //c.CronExpression = @"00 23 * * *";
            });
            services.AddCronJob<LegalDocumentationCronJob>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"30 8,20 * * *";
                //c.CronExpression = @"00 23 * * *";
            });
            services.AddCronJob<SkillJob>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"45 8,20 * * *";
                //c.CronExpression = @"00 23 * * *";
            });

            services.AddCronJob<For35Job>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"00 9,20 * * *";
                //c.CronExpression = @"00 23 * * *";
            });

            services.AddCronJob<PatternCalibrationJob>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"05 9,20 * * *";
                //c.CronExpression = @"00 23 * * *";
            });

            services.AddCronJob<PermissionCronjob>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"10 10,18 * * *";
                //c.CronExpression = @"00 23 * * *";
            });

            services.AddCronJob<MachInsuranceJob>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"00 11,15 * * *";
                //c.CronExpression = @"00 23 * * *";
            });

            services.AddCronJob<MachSoatJob>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"05 11,15 * * *";
                //c.CronExpression = @"00 23 * * *";
            });

            services.AddCronJob<MachTechnicalJob>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"10 11,15 * * *";
                //c.CronExpression = @"00 23 * * *";
            });

            services.AddCronJob<TransportInsuranceJob>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"15 11,15 * * *";
                //c.CronExpression = @"00 23 * * *";
            });

            services.AddCronJob<TransportSoatJob>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"20 11,15 * * *";
                //c.CronExpression = @"00 23 * * *";
            });

            services.AddCronJob<TransportTechnicalJob>(c =>
            {

                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"25 11,15 * * *";
                //c.CronExpression = @"00 23 * * *";
            });

            //services.AddCronJob<RacsJob>(c =>
            //{

            //    c.TimeZoneInfo = TimeZoneInfo.Local;
            //    c.CronExpression = @"*/5 7-18 * * *";
            //});

            services.Configure<FormOptions>(options => 
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
            });
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.LoginPath = "/ingresar";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            });
            //services.AddHttpsRedirection(options =>
            //{
            //    options.HttpsPort = 443;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IvcDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (ConstantHelpers.Seed.ENABLED)
            {
                DbInitializer.Seed(context, userManager, roleManager);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //// Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "IVC API V1");
            //    c.RoutePrefix = "api/docs";
            //});

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "accountlogin",
                   template: "Identity",
                   defaults: new { controller = "Account", action = "Login" });

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
