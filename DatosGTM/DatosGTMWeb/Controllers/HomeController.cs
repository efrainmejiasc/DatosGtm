
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using DatosGTMModelo.DataModel;
using System.Diagnostics.CodeAnalysis;
//using System.Web.Http.Filters;
using DatosGTMWeb.Filters;

namespace DatosGTMWeb.Controllers
{
    [CustomAuthenticationFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private Usuario usuario;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContext)
        {
            this._logger = logger;
            this._httpContext = httpContext;

            if (!string.IsNullOrEmpty(_httpContext.HttpContext.Session.GetString("UsuarioLogin")))
                this.usuario = JsonConvert.DeserializeObject<Usuario>(_httpContext.HttpContext.Session.GetString("UsuarioLogin"));
        }

        public IActionResult Index()
        {
            return View();
        }

   }
}