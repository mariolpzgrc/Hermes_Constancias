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
using Hermes2018.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Hermes2018.Services;
using Hermes2018.Resources;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Hermes2018.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hermes2018.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Logging;
using DinkToPdf;
using DinkToPdf.Contracts;
using Hermes2018.ModelsDBF;
using Hermes2018.Services.Interfaces;

namespace Hermes2018
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
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(12);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<Hermes2018.Models.HER_Usuario, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidIssuer = "",
                        ValidAudience = "",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConstApiMasterKey.Key)),
                        ClockSkew = TimeSpan.Zero,
                    });

            services.AddScoped<IUserClaimsPrincipalFactory<Hermes2018.Models.HER_Usuario>, AppClaimsPrincipalFactory>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                    options.RootDirectory = "/Content";
                })
                .AddJsonOptions(options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddDataAnnotationsLocalization(options => {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "_AppHermesv2.3";
                options.Cookie.Expiration = TimeSpan.FromHours(12);
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                options.ExpireTimeSpan = TimeSpan.FromHours(12);
                options.SlidingExpiration = false;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.LoginPath; //CookieAuthenticationDefaults.ReturnUrlParameter
                
                options.LoginPath = $"/Account/Login"; //$"/Identity/Account/Login"
                options.LogoutPath = $"/Account/Logout"; //$"/Identity/Account/Logout"
                options.AccessDeniedPath = $"/Account/AccessDenied"; //$"/Identity/Account/AccessDenied"
            });

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("es-MX"),
                    new CultureInfo("es-ES")
                };
                options.DefaultRequestCulture = new RequestCulture("es-ES", "es-ES");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "XSRF-TOKEN";
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 443;
            });

            services.AddHttpClient();

            services.AddTransient<IMainInitService, MainInitService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddScoped<IUsuarioClaimService, UsuarioClaimService>();
            services.AddTransient<INotificacionService, NotificacionService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IDocumentoService, DocumentoService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<IAreaService, AreaService>();
            services.AddTransient<IRolService, RolService>();
            services.AddTransient<IPlantillaService, PlantillaService>();
            services.AddTransient<IImportanciaService, ImportanciaService>();
            services.AddTransient<ITipoDocumentoService, TipoDocumentoService>();
            services.AddTransient<IAnexoService, AnexoService>();
            services.AddTransient<IGrupoService, GrupoService>();
            services.AddTransient<IServicioService, ServicioService>();
            services.AddTransient<IHistoricoService, HistoricoService>();
            services.AddTransient<IEstadisticasService, EstadisticasService>();
            services.AddTransient<ICarpetaService, CarpetaService>();
            services.AddTransient<ICalendarioService, CalendarioService>();
            services.AddTransient<IConfiguracionService, ConfiguracionService>();
            services.AddTransient<ISolicitudesService, SolicitudesService>();
            services.AddTransient<IDelegarService, DelegarService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<IEstadoEnvioService, EstadoEnvioService>();
            services.AddTransient<ITipoEnvioService, TipoEnvioService>();
            services.AddTransient<IHerramientaService, HerramientaService>();
            services.AddTransient<IOracleService, OracleService>();
            services.AddTransient<IVisibilidadService, VisibilidadService>();
            services.AddTransient<IRecopilacionService, RecopilacionService>();
            services.AddTransient<ITramiteService, TramiteService>();

            //constancias
            services.AddTransient<IConstanciaService, ConstanciaService>();
            services.AddTransient<IDescargarConstanciaPDFService, DescargarConstanciaPDFService>();

            //Servicio de convertidor de pdfbyte[] DescargarPDF();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            //DbContext Extra
            //Comando: Scaffold-DbContext -Connection "name=DefaultConnection" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir ModelsDBF -ContextDir Data -Context DBFContext -Tables CDEPEN,CDEPEN_EQUIV,PAREAS,PUSUARIOS -DataAnnotations -UseDatabaseNames -force
            services.AddDbContext<DBFContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Descarga del PDF
            services.AddTransient<IDescargarPDF, DescargarPDFService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMainInitService mainInit, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();

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

            loggerFactory.AddFile("Logs/log-{Date}.txt", minimumLevel: LogLevel.Error, retainedFileCountLimit: null);

            //Initialize Extra
            //mainInit.Inicial();
            //mainInit.UsuariosAsync().Wait();
            //cuando se hace una nueva migracion de base de datos descomentar esta linea y ejecutar el programa, una vez que ya se hayan registradocoomentarla
            //mainInit.Tramites();
        }
    }
}
