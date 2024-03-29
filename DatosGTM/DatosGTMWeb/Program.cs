using DatosGTMModelo.DataModel;
using DatosGTMModelo.IRepositories;
using DatosGTMModelo.Repositories;
using DatosGTMNegocio.Helpers;
using DatosGTMNegocio.IServices;
using DatosGTMNegocio.Services;
using DatosGTMWeb.Filters;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new CustomAuthenticationFilter());
    //config.Filters.Add(new CustomAuthorizationFilter());
});

builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddMvc();
#if DEBUG
builder.Services.AddDbContext<MyAppContext>(
    op => op.UseSqlServer(@"Server=EMCSERVERASUS\SQLEXPRESS;DataBase=DatosGTM;User Id=sa;Password=1234santiago;MultipleActiveResultSets=false;Connection Timeout=120;TrustServerCertificate=True;",
    b => b.MigrationsAssembly("DatosGTMModelo")));
#else 
builder.Services.AddDbContext<MyAppContext>(
    op => op.UseSqlServer(@"Server=SQL5101.site4now.net;DataBase=db_a8f8a1_datosgtm;User Id=db_a8f8a1_datosgtm_admin;Password=1234santiago;MultipleActiveResultSets=false;Connection Timeout=120;TrustServerCertificate=True;",
    b => b.MigrationsAssembly("DatosGTMModelo")));
#endif

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<MyAppContext, MyAppContext>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITerceroRepository, TerceroRepository>();
builder.Services.AddScoped<ITerceroService, TerceroService>();
builder.Services.AddScoped<IReadFileService, ReadFileService>();


AdobePdfApi.client_id = builder.Configuration["client_credentials:client_id"];
AdobePdfApi.client_secret = builder.Configuration["client_credentials:client_secret"];
AdobePdfApi.organization_id = builder.Configuration["service_account_credentials:organization_id"];
AdobePdfApi.account_id = builder.Configuration["service_account_credentials:account_id"];
AdobePdfApi.private_key_file = builder.Configuration["service_account_credentials:private_key_file"];
AdobePdfApi.pdf_filesToRead = builder.Configuration["ADOBE_PDF_API:pdf_filesToRead"];
AdobePdfApi.pdf_filesToWrite = builder.Configuration["ADOBE_PDF_API:pdf_filesToWrite"];
AdobePdfApi.pdf_filesExtract = builder.Configuration["ADOBE_PDF_API:pdf_filesExtract"];
AdobePdfApi.file_credentials = builder.Configuration["ADOBE_PDF_API:file_credentials"];
AdobePdfApi.log_excepcion = builder.Configuration["ADOBE_PDF_API:log_excepcion"];
AdobePdfApi.split_number_pages = Convert.ToInt32(builder.Configuration["ADOBE_PDF_API:split_number_pages"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.UseSession();
app.Run();

//******************************************************************************
var culturaInglesa = "en-US";
var culturaEspaņola = "es-ES";
var ci = new CultureInfo(culturaInglesa);
ci.NumberFormat.NumberDecimalSeparator = ".";
ci.NumberFormat.CurrencyDecimalSeparator = ".";
var ce = new CultureInfo(culturaEspaņola);
ce.NumberFormat.NumberDecimalSeparator = ",";
ce.NumberFormat.CurrencyDecimalSeparator = ",";
CultureInfo.DefaultThreadCurrentCulture = ci;
CultureInfo.DefaultThreadCurrentUICulture = ci;

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ci),
    SupportedCultures = new List<CultureInfo>
    {
     ci,ce
    },
    SupportedUICultures = new List<CultureInfo>
    {
    ci,ce
    }
});
//******************************************************************************


