using DatosGTMlWeb.Models;
using DatosGTMModelo.DataModel;
using DatosGTMNegocio.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DatosGTMWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUsuarioService _usuarioService;
        private  Usuario usuario;

        public LoginController(ILogger<HomeController> logger, IHttpContextAccessor httpContext, IUsuarioService usuarioService)
        {
            this._logger = logger;
            this._httpContext = httpContext;
            this._usuarioService = usuarioService;
            this.usuario = new Usuario();

            if (!string.IsNullOrEmpty(_httpContext.HttpContext.Session.GetString("UsuarioLogin")))
                this.usuario= JsonConvert.DeserializeObject<Usuario>(_httpContext.HttpContext.Session.GetString("UsuarioLogin"));
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult LoginUsuario(string userMail, string password)
        {
            var respuesta = new RespuestaModel();
            respuesta.Estado = false;
            respuesta.Mensaje = "No autorizado";
            if (string.IsNullOrEmpty(userMail) || string.IsNullOrEmpty(password))
                return Json(respuesta);

            var passwordEncriptado = DatosGTMNegocio.Helpers.Helper.EnCodeBase64(userMail + password);

            try
            {
                var gestor = this._usuarioService.GetUserData (userMail, passwordEncriptado);
                if (gestor != null)
                {
                    respuesta.Estado = true;
                    respuesta.Mensaje = "Autorizado";
                    _httpContext.HttpContext.Session.SetString("UsuarioLogin", JsonConvert.SerializeObject(gestor));
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }

            return Json(respuesta);
        }
    }
}
