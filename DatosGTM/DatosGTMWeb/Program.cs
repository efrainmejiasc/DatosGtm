using DatosGTMModelo.DataModel;
using DatosGTMModelo.IRepositories;
using DatosGTMModelo.Repositories;
using DatosGTMNegocio.Helpers;
using DatosGTMNegocio.IServices;
using DatosGTMNegocio.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

/*builder.Services.AddControllers(config =>
{
    config.Filters.Add(new CustomAuthenticationFilter());
    //config.Filters.Add(new CustomAuthorizationFilter());
});*/

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
builder.Services.AddDbContext<MyAppContext>(
    op => op.UseSqlServer(@"Server=EMCSERVERHP\SQLEXPRESS;DataBase=DatosGTM;User Id=sa;Password=1234santiago;MultipleActiveResultSets=false;Connection Timeout=120;TrustServerCertificate=True;",
    b => b.MigrationsAssembly("DatosGTMModelo")));


builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<MyAppContext, MyAppContext>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

AdobePdfApi.client_id = builder.Configuration["ADOBE_PDF_API:client_id"];
AdobePdfApi.client_secret = builder.Configuration["ADOBE_PDF_API:client_secret"];
AdobePdfApi.organization_id = builder.Configuration["ADOBE_PDF_API:organization_id"];
AdobePdfApi.account_id = builder.Configuration["ADOBE_PDF_API:account_id"];
AdobePdfApi.private_key_file = builder.Configuration["ADOBE_PDF_API:private_key_file"];


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


