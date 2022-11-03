using DatosGTMlWeb.Models;
using DatosGTMNegocio.Helpers;
using DatosGTMNegocio.IServices;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DatosGTMWeb.Controllers
{
    public class AboutController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRequestService _requestService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AboutController(ILogger<HomeController> logger, IHttpContextAccessor httpContext, IRequestService requestService, IWebHostEnvironment webHostEnvironment)
        {
            this._logger = logger;
            this._httpContext = httpContext;
            this._requestService = requestService;
            this._webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RequestAsync()
        {
            var respuesta = new RespuestaModel();
            respuesta.Estado = false;
            try
            {
                AdobePdfApi.private_key_filetext = Helper.ReadFile(this._webHostEnvironment.WebRootPath + AdobePdfApi.private_key_file);
                var keyCertificado = Helper.GetKeyCertificado(this._webHostEnvironment.WebRootPath + AdobePdfApi.certificado );
               
                respuesta.Mensaje = await this._requestService.ObtenerJWTAsync(AdobePdfApi.urlAdobePdfApiAutorizacion);
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Json(respuesta);
        }
    }
}
